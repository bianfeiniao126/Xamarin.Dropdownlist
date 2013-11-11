using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace sample.dropdownlist
{
	public class TestTableCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("TestTableCell");

		public TestTableCell () : base (UITableViewCellStyle.Value1, Key)
		{
			// TODO: add subviews to the ContentView, set various colors, etc.
			TextLabel.Text = "TextLabel";
		}
	}
}

