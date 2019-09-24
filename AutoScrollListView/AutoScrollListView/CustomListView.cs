﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace AutoScrollListView
{
    public class CustomListView : ListView
    {
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e?.NewItems != null)
            {
                int newItemCount = e.NewItems.Count;

                if (newItemCount > 0)
                {
                    ScrollIntoView(e.NewItems[newItemCount - 1]);
                }
            }

            base.OnItemsChanged(e);
        }
    }
}
