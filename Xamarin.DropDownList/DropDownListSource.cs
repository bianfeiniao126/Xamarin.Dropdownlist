using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Reflection;
using System.Drawing;

namespace Xamarin.DropDownList
{
	public class DropDownListSource<T> : UITableViewSource , IList<T>
	{
		public static readonly NSString Key = new NSString ("ItemCell");
		List<T> items = null;

		public List<T> Items {
			get {
				return items;
			}
			set {
				items = value;
			}
		}

		public string DataColumn = "Text";
		public string IdColumn = "Value";
		public string ImageColumn = "ImageName";
		T selectedItem = default(T);

		public T SelectedItem {
			get {
				return selectedItem;
			}
		}

		int selectedIndex = -1;

		public int SelectedIndex {
			get {
				return selectedIndex;
			}
		}

		string selectedText = string.Empty;

		public string SelectedText {
			get {
				return selectedText;
			}
		}

		object selectedValue = null;

		public object SelectedValue {
			get {
				return selectedValue;
			}
		}

		public DropDownListSource ()
		{

		}

		List<UITableViewCell> cells = new List<UITableViewCell> ();

		public List<UITableViewCell> Cells {
			get {
				return cells;
			}
		}

		float totalCellHeight = 0.0f;

		public float TotalCellHeight {
			get {
				return totalCellHeight;
			}
		}

		public delegate void TableCellsPopulatedHandler (float totalCellHeight);

		public event TableCellsPopulatedHandler TableCellsPopulated;

		void onTableCellsPopulated (float totalCellHeight)
		{
			if (TableCellsPopulated != null) {
				TableCellsPopulated (totalCellHeight);
			}
		}

		public delegate void TableCellRowSelectedHandler();
		public event TableCellRowSelectedHandler TableCellRowSelected;
		void onTableCellRowSelected()
		{
			if (TableCellRowSelected!=null) {
				TableCellRowSelected ();
			}
		}
		#region implemented abstract members of UITableViewSource

		public override int RowsInSection (UITableView tableview, int section)
		{
			if (items != null) {
				return items.Count;
			}
			return 0;
		}

		public override float GetHeightForRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = PrepareCell (tableView, indexPath);
			cells.Add (cell);
			totalCellHeight += cell.Frame.Height;
			return cell.Frame.Height;
		}

		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = cells [indexPath.Row];
			int totalVisibleRows = tableView.IndexPathsForVisibleRows.Length;
			if (indexPath.Row == totalVisibleRows-1) {
				onTableCellsPopulated (totalCellHeight);
			}
			return cell;
		}

		UITableViewCell PrepareCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (Key) as UITableViewCell;
			if (cell == null)
				cell = new UITableViewCell ();
			T currentRow = default(T);
			if (items != null) {
				currentRow = items [indexPath.Row];
			}
			Type typ = currentRow.GetType ();
			PropertyInfo pinfo = typ.GetProperty (DataColumn);
			object val = pinfo.GetValue (currentRow);
			cell.TextLabel.Text = val.ToString ();
			cell.TextLabel.Font = UIFont.SystemFontOfSize (12);

			try {
				string imageName = (string)currentRow.GetType ().GetProperty (ImageColumn).GetValue (currentRow);

				if (!string.IsNullOrEmpty(imageName)) {
					cell.ImageView.Image = UIImage.FromBundle(imageName);
				}
			} catch (Exception ex) {
				
			}

			cell.Frame = new System.Drawing.RectangleF (cell.Frame.X,
			                                           cell.Frame.Y,
			                                           cell.Frame.Width,
			                                           25.0f);

			return cell;
		}
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (cells!=null) {
				T item = items [indexPath.Row];
				selectedItem = item;
				selectedIndex = indexPath.Row;

				try {
					selectedText = (string)item.GetType ().GetProperty (DataColumn).GetValue (item);
					selectedValue = (string)item.GetType ().GetProperty (IdColumn).GetValue (item);

				} catch (Exception ex) {
					
				}
				onTableCellRowSelected ();
			}
		}

		#endregion

		#region IList implementation

		public int IndexOf (T item)
		{
			if (items != null) {
				return Items.IndexOf (item);	
			}
			return -1;
		}

		public void Insert (int index, T item)
		{
			if (items != null) {
				items.Insert (index, item);	
			}

		}

		public void RemoveAt (int index)
		{
			if (items != null) {
				items.RemoveAt (index);
			}

		}

		public T this [int index] {
			get {
				if (items != null) {
					return items [index];	
				}
				return default(T);
			}
			set {
				if (items != null) {
					items [index] = value;	
				}
			}
		}

		#endregion

		#region ICollection implementation

		public void Add (T item)
		{

			if (items == null) {
				items = new List<T> ();	
			}

			items.Add (item);
		}

		public void Clear ()
		{
			if (items != null) {
				items.Clear ();
			}
		}

		public bool Contains (T item)
		{
			if (items != null) {
				return items.Contains (item);
			}
			return false;
		}

		public void CopyTo (T[] array, int arrayIndex)
		{
			items.CopyTo (array, arrayIndex);
		}

		public bool Remove (T item)
		{
			return items.Remove (item);
		}

		public int Count {
			get {
				return items.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<T> GetEnumerator ()
		{
			return items.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return items.GetEnumerator ();
		}

		#endregion

	}
}

