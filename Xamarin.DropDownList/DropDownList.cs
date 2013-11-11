using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace Xamarin.DropDownList
{
	public enum DropDownDirectionValue
	{
		Down,
		Up
	}

	public class DropDownList<T>:UIView
	{
		UITextView txtData = new UITextView ();
		UIImage handleImage = null;
		UITableView tblDataView = null;
		float tableViewHeight = 0.0f;
		float defaultHeight = 32.0f;
		int visibleRows = 5;
		DropDownDirectionValue dropDownDirection = DropDownDirectionValue.Down;

		public DropDownDirectionValue DropDownDirection {
			get {
				return dropDownDirection;
			}
			set {
				dropDownDirection = value;
			}
		}

		UIView parentView = null;

		public int VisibleRows {
			get {
				return visibleRows;
			}
			set {
				visibleRows = value;
			}
		}

		public DropDownListSource<T> source = null;

		public DropDownListSource<T> Source {
			get {
				return source;
			}
			set {
				source = value;
				source.TableCellsPopulated += HandleTableCellsPopulated;
				source.TableCellRowSelected += HandleTableCellRowSelected;
				if (tblDataView != null) {
					tblDataView.Source = value;
				}
			}
		}

		void HandleTableCellRowSelected ()
		{
			if (txtData != null) {
				txtData.Text = source.SelectedText;
				ShowDropDownAnimated (tblDataView);
			}
		}

		public override void WillMoveToSuperview (UIView newsuper)
		{

			base.WillMoveToSuperview (newsuper);
			parentView = newsuper;
			newsuper.BringSubviewToFront (this);
		}

		void HandleTableCellsPopulated (float totalCellHeight)
		{
			tableViewHeight = totalCellHeight;
		}

		bool useDefaultFrame = true;

		public UIImage HandleImage {
			get {
				return handleImage;
			}
			set {
				handleImage = value;
				SetupView ();

			}
		}

		float textLabelFontSize = 11.0f;

		public float TextLabelFontSize {
			get {
				return textLabelFontSize;
			}
			set {
				textLabelFontSize = value;
				txtData.Font = UIFont.SystemFontOfSize (textLabelFontSize);
				txtData.Frame = new RectangleF (txtData.Frame.X,
				                                txtData.Frame.Y,
				                                txtData.Frame.Width,
				                                txtData.ContentSize.Height);


			}
		}
		UIImageView handleImageContainer =null;
		RectangleF defaultFrame = new RectangleF (0, 0, 200, 75);

		public virtual void FitImage()
		{
			handleImageContainer.Frame = new RectangleF (handleImageContainer.Frame.X,
			                                             handleImageContainer.Frame.Y,
			                                             handleImageContainer.Frame.Width,
			                                             txtData.ContentSize.Height);
			handleImageContainer.ContentMode = UIViewContentMode.Center;
			handleImageContainer.ClipsToBounds = true;
		}
		public DropDownList ():base()
		{
			SetupView ();
		}

		public DropDownList (RectangleF frame):base(frame)
		{
			useDefaultFrame = false;
			SetupView ();
		}

		public DropDownList (NSCoder coder):base(coder)
		{
			SetupView ();
		}

		public DropDownList (NSObjectFlag t):base(t)
		{
			SetupView ();
		}

		public DropDownList (IntPtr handle):base(handle)
		{
			SetupView ();
		}

		public virtual void SetupView ()
		{

			this.BackgroundColor = UIColor.Gray;
			if (handleImage == null) {
				handleImage = UIImage.FromBundle ("24");
			}

	

			if (txtData == null) {
				txtData = new UITextView ();
			}

			if (handleImage != null) {
				defaultHeight = handleImage.Size.Height;
				defaultFrame = new RectangleF (defaultFrame.X,
				                               defaultFrame.Y,
				                               defaultFrame.Width,
				                               handleImage.Size.Height + 2);
				if (useDefaultFrame) {
					base.Frame = defaultFrame;
				}

				txtData.Frame = new RectangleF (0, 0, base.Frame.Width - handleImage.Size.Width, handleImage.Size.Height);
			} else {
				defaultHeight = txtData.Frame.Height;
				defaultFrame = new RectangleF (defaultFrame.X,
				                               defaultFrame.Y,
				                               defaultFrame.Width,
				                               txtData.Frame.Height + 2);
				if (useDefaultFrame) {
					base.Frame = defaultFrame;
				}

				txtData.Frame = new RectangleF (0, 0, base.Frame.Width, txtData.Frame.Height);
			}


			txtData.BackgroundColor = UIColor.LightGray;

			handleImageContainer = new UIImageView (new RectangleF (txtData.Frame.Width , 0, handleImage.Size.Width, defaultHeight));
			handleImageContainer.Image = handleImage;
			handleImageContainer.UserInteractionEnabled = true;

			//Add support for image arrow tapped
			UITapGestureRecognizer singleTap = new UITapGestureRecognizer (new NSAction (delegate() {
				//Display tableview
				ShowDropDownAnimated (tblDataView);
			}));
			singleTap.NumberOfTapsRequired = 1;
			singleTap.NumberOfTouchesRequired = 1;

			this.AddSubview (txtData);
			this.AddSubview (handleImageContainer);
			handleImageContainer.AddGestureRecognizer (singleTap);


			handleImageContainer.BackgroundColor = UIColor.Red;


			tblDataView = new UITableView (new RectangleF (0, handleImage.Size.Height, base.Frame.Width, visibleRows * 25));
			tblDataView.Hidden = true;
			this.AddSubview (tblDataView);

			this.SendSubviewToBack (tblDataView); 


		}

		public void Redraw ()
		{
			SetupView ();
		}

		void ShowDropDownAnimated (UITableView tableView)
		{
			float visibleRowHeight = 0.0f;
			if (handleImage.Size.Height > txtData.Frame.Height) {
				defaultHeight = handleImage.Size.Height;
			} else {
				defaultHeight = txtData.Frame.Height;
			}

			if (tableView.Hidden) {

				if (tableViewHeight == 0) {
					tableViewHeight = source.TotalCellHeight;
				}
				//Setup Visible Rows Height
				if (visibleRows > 0) {
					for (int i = 0; i < visibleRows; i++) {
						if (source.Cells.Count > i) {
							visibleRowHeight += source.Cells [i].Frame.Height;	
						}
					}
				} else
					visibleRowHeight = tableViewHeight;

				if (tableViewHeight > visibleRowHeight) {
					tableViewHeight = visibleRowHeight;
				}
				if (dropDownDirection == DropDownDirectionValue.Down) {
					tableView.Frame = new RectangleF (tableView.Frame.X, txtData.Frame.Y + txtData.Frame.Height, tableView.Frame.Width, 0);	
				}
				else if (dropDownDirection == DropDownDirectionValue.Up) {
					tableView.Frame = new RectangleF (tableView.Frame.X, txtData.Frame.Y, tableView.Frame.Width, 0);
				}

				Console.WriteLine(tableView.Frame.Y);
				//base.Frame = new RectangleF (base.Frame.X, base.Frame.Y, base.Frame.Width+0.5f, defaultHeight+0.5f);
				tblDataView.Hidden = false;
				UIView.Animate (0.25, delegate() {
					if (dropDownDirection == DropDownDirectionValue.Down) {
						base.Frame = new RectangleF (base.Frame.X - 0.5f, base.Frame.Y, base.Frame.Width + 1, defaultHeight + tableViewHeight + 0.5f);
						tableView.Frame = new RectangleF (tableView.Frame.X,txtData.Frame.Y + txtData.Frame.Height, tableView.Frame.Width, tableViewHeight);
					} else if (dropDownDirection == DropDownDirectionValue.Up) {
						//base.Frame = new RectangleF (base.Frame.X - 0.5f, base.Frame.Y, base.Frame.Width + 1, defaultHeight + tableViewHeight + 0.5f);
						tableView.Frame = new RectangleF (tableView.Frame.X, txtData.Frame.Y - tableViewHeight, tableView.Frame.Width, tableViewHeight);
					}
				},
				delegate {
					Console.WriteLine(tableView.Frame.Y);
				});
			} else {
				UIView.Animate (0.25, delegate() {
					if (dropDownDirection == DropDownDirectionValue.Down) {
						base.Frame = new RectangleF (base.Frame.X + 0.5f, base.Frame.Y, base.Frame.Width - 1, defaultHeight - 0.5f);
						tableView.Frame = new RectangleF (tableView.Frame.X, txtData.Frame.Y + txtData.Frame.Height, tableView.Frame.Width, defaultHeight);
					}
					else if (dropDownDirection == DropDownDirectionValue.Up) {
						//base.Frame = new RectangleF (base.Frame.X - 0.5f, base.Frame.Y, base.Frame.Width + 1, defaultHeight + tableViewHeight + 0.5f);
						tableView.Frame = new RectangleF (tableView.Frame.X, tableView.Frame.Y + tableViewHeight, tableView.Frame.Width, defaultHeight);
					}

				}, delegate {
					tableView.Hidden = true;
					Console.WriteLine(tableView.Frame.Y);
				});
			}
		}
	}
}

