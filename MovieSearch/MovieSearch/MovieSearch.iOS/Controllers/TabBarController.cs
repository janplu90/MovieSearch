using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieSearch.iOS.Controllers
{
    public class TabBarController : UITabBarController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.TabBar.BackgroundColor = UIColor.DarkGray;
            this.TabBar.TintColor = UIColor.Black;
            this.SelectedIndex = 0;
        }
    }
}
