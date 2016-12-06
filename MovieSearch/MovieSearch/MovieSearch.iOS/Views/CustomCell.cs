using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieSearch.iOS.Views
{
    public class CustomCell : UITableViewCell
    {
        private UILabel _nameLabel, _yearLabel, _castLabel;
        private UIImageView _imageView;

        

        public CustomCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            this._imageView = new UIImageView()
            {
                BackgroundColor = UIColor.Black
            };
            this._nameLabel = new UILabel()
            {
                Font = UIFont.PreferredSubheadline,
                TextColor = UIColor.White,
                BackgroundColor = UIColor.Black
            };

            this._yearLabel = new UILabel()
            {
                Font = UIFont.PreferredSubheadline,
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Center, //puting the text in the center of label
                BackgroundColor = UIColor.Clear
            };

            this._castLabel = new UILabel()
            {
                Font = UIFont.PreferredFootnote,
                TextColor = UIColor.Cyan,
                TextAlignment = UITextAlignment.Left, //puting the text in the center of label
                BackgroundColor = UIColor.Black
            };
            this.ContentView.AddSubviews(new UIView[] { this._imageView, this._nameLabel, this._castLabel });               
        }

        public override void LayoutSubviews() //dynamically change during the moving the screen, for example new load every screen move, DidLoad load only once
        {
            base.LayoutSubviews();

            this.BackgroundColor = UIColor.Black;
            
            this._imageView.Frame = new CGRect(5, 5, 40, 35);
            this._nameLabel.Frame = new CGRect(50, 5, this.ContentView.Bounds.Width - 30, 25);
            this._castLabel.Frame = new CGRect(50, 23, this.ContentView.Bounds.Width - 30, 20);
        }

        public void UpdateCell(string name, string year, string cast, string imageName)
        {
            this._imageView.Image = UIImage.FromFile(imageName);
            this._nameLabel.Text = name + " " + year;
            this._castLabel.Text = cast;
            this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
        }
    }
}
