﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
    {
        public IUnitOfWork Current => GetCurrentUnitOfWork();

        private readonly IServiceProvider _serviceProvider;
        private readonly IAmbientUnitOfWork _ambientUnitOfWork;
        private readonly UnitOfWorkDefaultOptions _defaultOptions;

        public UnitOfWorkManager(
            IServiceProvider serviceProvider,
            IAmbientUnitOfWork ambientUnitOfWork,
            IOptions<UnitOfWorkDefaultOptions> options)
        {
            _serviceProvider = serviceProvider;
            _ambientUnitOfWork = ambientUnitOfWork;
            _defaultOptions = options.Value;
        }

        public IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false)
        {
            Check.NotNull(options, nameof(options));

            var currentUow = Current;
            options = _defaultOptions.Normalize(options.Clone());
            if (currentUow != null && !requiresNew)
            {
                return new ChildUnitOfWork(currentUow, options);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Initialize(options);

            return unitOfWork;
        }

        public IUnitOfWork Reserve(string reservationName, bool requiresNew = false)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var options = _defaultOptions.Normalize(new UnitOfWorkOptions());
            if (!requiresNew &&
                _ambientUnitOfWork.UnitOfWork != null &&
                _ambientUnitOfWork.UnitOfWork.IsReservedFor(reservationName))
            {
                return new ChildUnitOfWork(_ambientUnitOfWork.UnitOfWork, options);
            }

            var unitOfWork = CreateNewUnitOfWork();
            unitOfWork.Reserve(reservationName, options);

            return unitOfWork;
        }

        public void BeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            if (!TryBeginReserved(reservationName, options))
            {
                throw new AbpException($"Could not find a reserved unit of work with reservation name: {reservationName}");
            }
        }

        public bool TryBeginReserved(string reservationName, UnitOfWorkOptions options)
        {
            Check.NotNull(reservationName, nameof(reservationName));

            var uow = _ambientUnitOfWork.UnitOfWork;

            //Find reserved unit of work starting from current and going to outers
            while (uow != null && !uow.IsReservedFor(reservationName))
            {
                uow = uow.Outer;
            }

            if (uow == null)
            {
                return false;
            }

            options = _defaultOptions.Normalize(options.Clone());
            uow.Initialize(options);

            return true;
        }

        private IUnitOfWork GetCurrentUnitOfWork()
        {
            var uow = _ambientUnitOfWork.UnitOfWork;

            //Skip reserved unit of work
            while (uow != null && (uow.IsReserved || uow.IsDisposed || uow.IsCompleted))
            {
                //使用最外层保留的UOW
                if (uow.Outer == null && uow.IsReserved) break;
                uow = uow.Outer;
            }

            return uow;
        }

        private IUnitOfWork CreateNewUnitOfWork()
        {
            var scope = _serviceProvider.CreateScope();
            try
            {
                var outerUow = _ambientUnitOfWork.UnitOfWork;

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                unitOfWork.SetOuter(outerUow);

                _ambientUnitOfWork.SetUnitOfWork(unitOfWork);

                unitOfWork.Disposed += (sender, args) =>
                {
                    _ambientUnitOfWork.SetUnitOfWork(outerUow);
                    scope.Dispose();
                };

                return unitOfWork;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}