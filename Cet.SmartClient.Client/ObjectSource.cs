using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Cet.SmartClient.Client
{
    public class ObjectSource : DependencyObject
    {
        #region Source DP
        //We don't know what will be the Source/target type so we keep 'object'.
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(object), typeof(ObjectSource));
          //new PropertyMetadata()
          //{
              
          //    BindsTwoWayByDefault = true,
          //    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
          //});
        public Object Source
        {

            get { return GetValue(ObjectSource.SourceProperty); }
            set { SetValue(ObjectSource.SourceProperty, value); }
        }
        #endregion

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);
            //if (e.Property.Name == ObjectSource.SourceProperty.Name)
            //{
            //    //no loop wanted
            //    if (!object.ReferenceEquals(Source, Target))
            //        Target = Source;
            //}
            //else if (e.Property.Name == ObjectSource.TargetProperty.Name)
            //{
            //    //no loop wanted
            //    if (!object.ReferenceEquals(Source, Target))
            //        Source = Target;
            //}
        //}

    }
}
