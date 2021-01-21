namespace TikzFix
{
    /// <summary>
    /// Wraps values so that they can be used in binding expressions
    /// </summary>
    /// <example>
    /// Consider binding of 2D array of doubles, WPF has no way of specifing such a binding that would 
    /// allow for two-way bindings, because WPF will simply copy values. Enter this PrimitiveWrapper that
    /// allows you for ease boxing of primitive variables so they can be used in collection binding expressions, because Two-Way requires property
    /// 
    /// something like this without wrapper is not possible
    ///<ListBox ItemsSource="{Binding RelativeSource={RelativeSource AncestorType=local:MatrixEditorControl}, Path=DataContext.Matrix, Mode=TwoWay}">
    ///     <ListBox.ItemTemplate>
    ///         <DataTemplate>
    ///         <ListBox ItemsSource = "{Binding Path=., Mode=TwoWay}" >
    ///             < ListBox.ItemTemplate >
    ///             < DataTemplate >
    ///                 < TextBox Text="{Binding Path=., Mode=TwoWay}"></TextBox> <-- This will only work in one direction if using primitive value
    ///             </DataTemplate>
    ///         ... rest of code
    ///         
    /// 
    /// 
    /// 
    /// ProTip: WPF is fucked
    /// </example>
    /// <typeparam name="T"></typeparam>
    public class PrimitiveWrapper<T> : BaseVM where T : struct
    {
        private T val;
        public T Value
        {
            get => val;
            set => SetProperty(ref val, value);
        }


        public PrimitiveWrapper(T value)
        {
            Value = value;
        }

        public PrimitiveWrapper()
        {
            Value = default(T);
        }
    }
}
