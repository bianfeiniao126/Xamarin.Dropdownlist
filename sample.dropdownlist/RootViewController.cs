using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.DropDownList;

namespace sample.dropdownlist
{
	public partial class RootViewController : UIViewController
	{
		public RootViewController () : base ("RootViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			DropDownListSource<ListItem> source = new DropDownListSource<ListItem> ();
			ListItem item0 = new ListItem () { Text = "Item0Item0Item0Item0Item0Item0Item0", Value = 0 };
			ListItem item1 = new ListItem () { Text = "Item1", Value = 1 };
			ListItem item2 = new ListItem () { Text = "Item2", Value = 2 };
			ListItem item3 = new ListItem () { Text = "Item3", Value = 2, ImageName ="32" };
			ListItem item4 = new ListItem () { Text = "Item4", Value = 2 };
			ListItem item5 = new ListItem () { Text = "Item5", Value = 2 };
			ListItem item6 = new ListItem () { Text = "Item6", Value = 2 };
			ListItem item7 = new ListItem () { Text = "Item7", Value = 2 };
			ListItem item8 = new ListItem () { Text = "Item8", Value = 2 };

			source.Add (item0);
			source.Add (item1);
			source.Add (item2);
			source.Add (item3);
			source.Add (item4);
			source.Add (item5);
			source.Add (item6);
			source.Add (item7);
			source.Add (item8);


			UILabel lab1 = new UILabel (new RectangleF (0, 75, 200, 50));
			//lab1.Text = "Sample text for test";
			this.View.AddSubview (lab1);

			//DropDownList<ListItem> dropDownList = new DropDownList<ListItem>(new System.Drawing.RectangleF (25,325,150,40));
			DropDownList<ListItem> dropDownList = new DropDownList<ListItem> (new System.Drawing.RectangleF (25,125,150,40));
			dropDownList.Source = source;
			//dropDownList.HandleImage = UIImage.FromBundle ("");
			dropDownList.DropDownDirection = DropDownDirectionValue.Down; //Default direction
			//dropDownList.DropDownDirection = DropDownDirectionValue.Up;

			//When used no of visible rows will be set based on this value. Defaults to 5
			//dropDownList.VisibleRows = 2;

			//If default font size modified, then need to set an image with matching height, dropdownlist has to intansitated with frame value
			dropDownList.TextLabelFontSize = 15.0f;
			this.View.AddSubview (dropDownList);





			//this.View.BringSubviewToFront (dropDownList);
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}

	/// <summary>
	/// Custom data source. Usage:dropDownList.Source = MySource
	/// <object> can be any class with custom properties to handle display data
	/// </summary>
	public class MySource:DropDownListSource<object>
	{
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			return base.GetCell (tableView, indexPath);
		}
	}
}

