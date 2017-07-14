using Sitecore.Install.Filters;
using Sitecore.Install.Framework;
using Sitecore.Shell.Applications.Install;
using Sitecore.Shell.Applications.Install.Controls;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.XmlControls;
using System;

namespace Sitecore.Support.Shell.Applications.Install.Controls
{
    public class ItemUserFilterEditor : XmlControl, IFilterEditorControl, IEditor
    {
        private ItemUserFilter _filter;
        protected XmlControl AccountSelector;
        protected Button ClearButton;

        public void BindTo(object instance)
        {
            if (instance is ItemUserFilter)
            {
                this._filter = instance as ItemUserFilter;
                this.SelectAccountControl.Value = this._filter.Accounts;
            }
        }

        public void ClearFilter()
        {
            this.SelectAccountControl.Value = string.Empty;
            if (this._filter != null)
            {
                this._filter.Accounts = string.Empty;
            }
        }

        public IFilter CreateFilter()
        {
            return new ItemUserFilter { FilterType = this.FilterType };
        }

        private string GetID(string id)
        {
            return (this.ID + "_" + id);
        }

        public bool Matches(object instance)
        {
            return ((instance is ItemUserFilter) && ((instance as ItemUserFilter).FilterType == this.FilterType));
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                this.AccountSelector.ID = this.GetID("AccountSelector");
                if (this._filter != null)
                {
                    this.SelectAccountControl.Value = this._filter.Accounts;
                }
            }
            this.ClearButton.Click = this.ID + ".ClearFilter";
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this._filter != null)
            {
                this._filter.Accounts = this.SelectAccountControl.Value;
            }
        }

        public ItemUserFilter.ItemUserFilterType FilterType
        {
            get
            {
                string str = StringUtil.GetString(this.ViewState["filterType"]);
                if (string.IsNullOrEmpty(str))
                {
                    return ItemUserFilter.ItemUserFilterType.Created;
                }
                return (ItemUserFilter.ItemUserFilterType)Enum.Parse(typeof(ItemUserFilter.ItemUserFilterType), str);
            }
            set
            {
                this.ViewState["filterType"] = value.ToString();
            }
        }

        private SelectAccount SelectAccountControl
        {
            get
            {
                return (this.AccountSelector as SelectAccount);
            }
        }
    }
}
