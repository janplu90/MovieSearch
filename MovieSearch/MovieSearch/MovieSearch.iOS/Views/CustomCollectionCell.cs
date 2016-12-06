using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieSearch.iOS.Views
{
    class CustomCollectionCell : UICollectionViewCell
    {
        private UILabel _nameLabel;
        private UIImageView _imageView;

        [Export("initWithFrame:")]
        public CustomCollectionCell(CGRect frame) : base(frame)
        {
            this._imageView = new UIImageView();
            this._nameLabel = new UILabel()
            {
                BackgroundColor = UIColor.Black,
                Font = UIFont.PreferredFootnote,
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Left
            };

    
            this.ContentView.AddSubviews(new UIView[] { this._imageView, this._nameLabel});
        }

        public override void LayoutSubviews() //dynamically change during the moving the screen, for example new load every screen move, DidLoad load only once
        {
            base.LayoutSubviews();

            this._imageView.Frame = new CGRect(0, 0, this.ContentView.Bounds.Width - 20, this.ContentView.Bounds.Height - 20);
            this._nameLabel.Frame = new CGRect(0,this.ContentView.Bounds.Height - 20  , this.ContentView.Bounds.Width,  20);
     
        }

        public void UpdateCell(string name, string year, string imageName)
        {
            this._imageView.Image = UIImage.FromFile(imageName);
            this._nameLabel.Text = name + year;
     
        }
    }
}
