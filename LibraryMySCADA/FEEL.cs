using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FEEL
{
    //перемещение на передний план
    public static class FrameworkElementExt
    {
       // static Window based;
        public static void BringToFront(this FrameworkElement element)
        {
            if (element == null) return;

            Panel parent = element.Parent as Panel;
            if (parent == null) return;

            var maxZ = parent.Children.OfType<UIElement>()
              .Where(x => x != element)
              .Select(x => Panel.GetZIndex(x))
              .Max();
            Panel.SetZIndex(element, maxZ + 1);
        }
    }

}
