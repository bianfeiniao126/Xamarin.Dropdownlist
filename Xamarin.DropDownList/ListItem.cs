using System;
using MonoTouch.UIKit;

namespace Xamarin.DropDownList
{
	public class ListItem
	{
		public ListItem ()
		{
		}

		string imageName;

		public string ImageName {
			get {
				return imageName;
			}
			set {
				imageName = value;
			}
		}

		public string text;

		public string Text {
			get {
				return text;
			}
			set {
				text = value;
			}
		}

		public int value;

		public int Value {
			get {
				return value;
			}
			set {
				value = value;
			}
		}
	}
}

