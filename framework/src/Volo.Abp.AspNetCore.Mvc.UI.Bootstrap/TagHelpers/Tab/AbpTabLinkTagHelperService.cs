using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Extensions;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabLinkTagHelperService : AbpTagHelperService<AbpTabLinkTagHelper>
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            SetPlaceholderForNameIfNotProvided();
            
            var tabHeader = GetTabHeaderItem(context, output);

            var tabHeaderItems = context.GetValue<List<TabItem>>(TabItems);

            tabHeaderItems.Add(new TabItem(tabHeader, "", false, this.TagHelper.Name, this.TagHelper.ParentDropdownName, false));

            output.SuppressOutput();
            await Task.CompletedTask;
        }

        protected virtual string GetTabHeaderItem(TagHelperContext context, TagHelperOutput output)
        {
            var id = this.TagHelper.Name + "-tab";
            var href = this.TagHelper.Href;
            var title = this.TagHelper.Title;

            if (!string.IsNullOrWhiteSpace(this.TagHelper.ParentDropdownName))
            {
                return "<a class=\"dropdown-item\" id=\"" + id + "\" href=\"" + href + "\">" + title + "</a>";
            }

            return "<li class=\"nav-item\"><a class=\"nav-link" + AbpTabItemActivePlaceholder + "\" id=\"" + id + "\" href=\"" + href + "\">" +
                   title +
                   "</a></li>";
        }

        protected virtual void SetPlaceholderForNameIfNotProvided()
        {
            if (string.IsNullOrWhiteSpace(this.TagHelper.Name))
            {
                this.TagHelper.Name = TabItemNamePlaceHolder;
            }
        }
    }
}