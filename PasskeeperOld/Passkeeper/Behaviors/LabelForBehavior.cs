namespace Passkeeper.Behaviors;
public static class LabelForBehavior
{
    public static readonly BindableProperty TargetIdProperty =
        BindableProperty.CreateAttached(
            "TargetId",
            typeof(string),
            typeof(LabelForBehavior),
            default(string),
            propertyChanged: OnTargetIdChanged);

    public static string GetTargetId(BindableObject view)
    {
        return (string)view.GetValue(TargetIdProperty);
    }

    public static void SetTargetId(BindableObject view, string value)
    {
        view.SetValue(TargetIdProperty, value);
    }

    private static void OnTargetIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Label label && newValue is string targetId)
        {
            label.GestureRecognizers.Clear();
            label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (label.Parent is Layout layout)
                    {
                        View target = layout.FindByName<View>(targetId);
                        if (target is Entry entry)
                            entry.Focus();
                        else if (target is Switch sw)
                            sw.IsToggled = !sw.IsToggled;
                        // Extend for other control types as needed
                    }
                })
            });
        }
    }
}