using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DVLD.Classess.UIItems
{
    internal class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        public SortableBindingList() : base()
        {

        }

        public SortableBindingList(IList <T> list) : base(list)
        {

        }

        protected override bool SupportsSortingCore => true;
        protected override bool IsSortedCore => isSorted;

        protected override PropertyDescriptor SortPropertyCore => sortProperty;
        protected override ListSortDirection SortDirectionCore => sortDirection;

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (Items is List<T> ItemList)
            {

                Comparison<T> comparison = (x, y) =>
                {
                     object xValue = prop.GetValue(x);
                     object yValue = prop.GetValue(y);

                     int result;

                     if (xValue == null && yValue == null)
                         result = 0;
                     else if (xValue == null)
                         result = -1;
                     else if (yValue == null)
                         result = 1;

                     else
                         result = Comparer<object>.Default.Compare(xValue, yValue);

                     return direction == ListSortDirection.Ascending ? result : -result;
                };

                sortProperty = prop;
                sortDirection = direction;
                isSorted = true;

                ItemList.Sort(comparison);
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            else
            {
                isSorted = false;
            }
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
        }
    }
}
