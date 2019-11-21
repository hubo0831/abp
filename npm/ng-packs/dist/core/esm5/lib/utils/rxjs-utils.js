/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/rxjs-utils.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
/**
 * @param {?} value
 * @return {?}
 */
function isFunction(value) {
    return typeof value === 'function';
}
/** @type {?} */
export var takeUntilDestroy = (/**
 * @param {?} componentInstance
 * @param {?=} destroyMethodName
 * @return {?}
 */
function (componentInstance, destroyMethodName) {
    if (destroyMethodName === void 0) { destroyMethodName = 'ngOnDestroy'; }
    return (/**
     * @template T
     * @param {?} source
     * @return {?}
     */
    function (source) {
        /** @type {?} */
        var originalDestroy = componentInstance[destroyMethodName];
        if (isFunction(originalDestroy) === false) {
            throw new Error(componentInstance.constructor.name + " is using untilDestroyed but doesn't implement " + destroyMethodName);
        }
        if (!componentInstance['__takeUntilDestroy']) {
            componentInstance['__takeUntilDestroy'] = new Subject();
            componentInstance[destroyMethodName] = (/**
             * @return {?}
             */
            function () {
                // tslint:disable-next-line: no-unused-expression
                isFunction(originalDestroy) && originalDestroy.apply(this, arguments);
                componentInstance['__takeUntilDestroy'].next(true);
                componentInstance['__takeUntilDestroy'].complete();
            });
        }
        return source.pipe(takeUntil(componentInstance['__takeUntilDestroy']));
    });
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicnhqcy11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9yeGpzLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFjLE9BQU8sRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUMzQyxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7Ozs7O0FBRTNDLFNBQVMsVUFBVSxDQUFDLEtBQUs7SUFDdkIsT0FBTyxPQUFPLEtBQUssS0FBSyxVQUFVLENBQUM7QUFDckMsQ0FBQzs7QUFFRCxNQUFNLEtBQU8sZ0JBQWdCOzs7OztBQUFHLFVBQUMsaUJBQWlCLEVBQUUsaUJBQWlDO0lBQWpDLGtDQUFBLEVBQUEsaUNBQWlDOzs7Ozs7SUFBSyxVQUN4RixNQUFxQjs7WUFFZixlQUFlLEdBQUcsaUJBQWlCLENBQUMsaUJBQWlCLENBQUM7UUFDNUQsSUFBSSxVQUFVLENBQUMsZUFBZSxDQUFDLEtBQUssS0FBSyxFQUFFO1lBQ3pDLE1BQU0sSUFBSSxLQUFLLENBQ1YsaUJBQWlCLENBQUMsV0FBVyxDQUFDLElBQUksdURBQWtELGlCQUFtQixDQUMzRyxDQUFDO1NBQ0g7UUFDRCxJQUFJLENBQUMsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsRUFBRTtZQUM1QyxpQkFBaUIsQ0FBQyxvQkFBb0IsQ0FBQyxHQUFHLElBQUksT0FBTyxFQUFFLENBQUM7WUFFeEQsaUJBQWlCLENBQUMsaUJBQWlCLENBQUM7OztZQUFHO2dCQUNyQyxpREFBaUQ7Z0JBQ2pELFVBQVUsQ0FBQyxlQUFlLENBQUMsSUFBSSxlQUFlLENBQUMsS0FBSyxDQUFDLElBQUksRUFBRSxTQUFTLENBQUMsQ0FBQztnQkFDdEUsaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUM7Z0JBQ25ELGlCQUFpQixDQUFDLG9CQUFvQixDQUFDLENBQUMsUUFBUSxFQUFFLENBQUM7WUFDckQsQ0FBQyxDQUFBLENBQUM7U0FDSDtRQUNELE9BQU8sTUFBTSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUksaUJBQWlCLENBQUMsb0JBQW9CLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFDNUUsQ0FBQztDQUFBLENBQUEiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBPYnNlcnZhYmxlLCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IHRha2VVbnRpbCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuXHJcbmZ1bmN0aW9uIGlzRnVuY3Rpb24odmFsdWUpIHtcclxuICByZXR1cm4gdHlwZW9mIHZhbHVlID09PSAnZnVuY3Rpb24nO1xyXG59XHJcblxyXG5leHBvcnQgY29uc3QgdGFrZVVudGlsRGVzdHJveSA9IChjb21wb25lbnRJbnN0YW5jZSwgZGVzdHJveU1ldGhvZE5hbWUgPSAnbmdPbkRlc3Ryb3knKSA9PiA8VD4oXHJcbiAgc291cmNlOiBPYnNlcnZhYmxlPFQ+XHJcbikgPT4ge1xyXG4gIGNvbnN0IG9yaWdpbmFsRGVzdHJveSA9IGNvbXBvbmVudEluc3RhbmNlW2Rlc3Ryb3lNZXRob2ROYW1lXTtcclxuICBpZiAoaXNGdW5jdGlvbihvcmlnaW5hbERlc3Ryb3kpID09PSBmYWxzZSkge1xyXG4gICAgdGhyb3cgbmV3IEVycm9yKFxyXG4gICAgICBgJHtjb21wb25lbnRJbnN0YW5jZS5jb25zdHJ1Y3Rvci5uYW1lfSBpcyB1c2luZyB1bnRpbERlc3Ryb3llZCBidXQgZG9lc24ndCBpbXBsZW1lbnQgJHtkZXN0cm95TWV0aG9kTmFtZX1gXHJcbiAgICApO1xyXG4gIH1cclxuICBpZiAoIWNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXSkge1xyXG4gICAgY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddID0gbmV3IFN1YmplY3QoKTtcclxuXHJcbiAgICBjb21wb25lbnRJbnN0YW5jZVtkZXN0cm95TWV0aG9kTmFtZV0gPSBmdW5jdGlvbigpIHtcclxuICAgICAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby11bnVzZWQtZXhwcmVzc2lvblxyXG4gICAgICBpc0Z1bmN0aW9uKG9yaWdpbmFsRGVzdHJveSkgJiYgb3JpZ2luYWxEZXN0cm95LmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XHJcbiAgICAgIGNvbXBvbmVudEluc3RhbmNlWydfX3Rha2VVbnRpbERlc3Ryb3knXS5uZXh0KHRydWUpO1xyXG4gICAgICBjb21wb25lbnRJbnN0YW5jZVsnX190YWtlVW50aWxEZXN0cm95J10uY29tcGxldGUoKTtcclxuICAgIH07XHJcbiAgfVxyXG4gIHJldHVybiBzb3VyY2UucGlwZSh0YWtlVW50aWw8VD4oY29tcG9uZW50SW5zdGFuY2VbJ19fdGFrZVVudGlsRGVzdHJveSddKSk7XHJcbn07XHJcbiJdfQ==