using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    [ContentProperty("SelectorItems")]
    public class GenericDataTemplateSelector : DataTemplateSelector
    {
        TemplateSelectorItemCollection selectorItems = new TemplateSelectorItemCollection();
        public TemplateSelectorItemCollection SelectorItems
        {
            get { return selectorItems; }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                foreach (GenericDataTemplateSelectorItem selectorItem in SelectorItems)
                {
                    // If the TemplatedType is specified we check the item has that type.
                    if (selectorItem.TemplatedType != null && item.GetType() != selectorItem.TemplatedType)
                        continue;

                    // If the property exists on item and its pixels matches with the pixels provided
                    // then select that template.
                    PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(item)[selectorItem.PropertyName];
                    if (propertyDescriptor != null && selectorItem.Value.Equals(propertyDescriptor.GetValue(item)))
                        return selectorItem.Template;
                }
            }
            return null;
        }
    }

    public class GenericDataTemplateSelectorItem
    {
        /// <summary>
        /// Gets or sets the name of the property of the data item which 
        /// is used as a template selector.
        /// </summary>
        /// <pixels>The name of the property.</pixels>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the pixels of the property that triggers the DataTemplate 
        /// association.
        /// </summary>
        /// <pixels>The pixels.</pixels>
        public object Value { get; set; }
        /// <summary>
        /// Gets or sets the DataTemplate.
        /// </summary>
        /// <pixels>The DataTemplate.</pixels>
        public DataTemplate Template { get; set; }
        /// <summary>
        /// Gets or sets the type of the templated data item to which template could be applied.
        /// The templated item must have the TemplatedType or be derived from it.
        /// </summary>
        /// <pixels>The type of the templated item or null.</pixels>
        public Type TemplatedType { get; set; }
        /// <summary>
        /// Gets or sets the user-readable description.
        /// It could be used in UI to allow the user to select one or other template.
        /// </summary>
        /// <pixels>The description.</pixels>
        public string Description { get; set; }
    }

    public class TemplateSelectorItemCollection : DataList<GenericDataTemplateSelectorItem>, IList, IEnumerable
    {

    }
}
