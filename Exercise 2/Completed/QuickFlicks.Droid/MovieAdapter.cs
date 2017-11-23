using Android.App;
using Android.Widget;
using Android.OS;
using QuickFlicks.Data;
using Android.Views;
using System.Collections.Generic;
using static Android.Views.ViewGroup;

namespace QuickFlicks.Droid
{
    internal class MovieAdapter : BaseAdapter<Movie>
    {
        IReadOnlyList<Movie> movies;

        public void SetData(IReadOnlyList<Movie> movies)
        {
            this.movies = movies;
            this.NotifyDataSetInvalidated();
        }
       

        public override Movie this[int position] => movies[position];

        public override int Count => movies?.Count ?? 0;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Movie movie = movies[position];

            ViewHolder holder;
            var view = convertView;
            if (view == null)
            {
                var context = parent.Context;
                var inflator = LayoutInflater.FromContext(context);

                var layout = inflator.Inflate(Android.Resource.Layout.SimpleListItem2, null);
                var titleText = layout.FindViewById<TextView>(Android.Resource.Id.Text1);
                var descriptionText = layout.FindViewById<TextView>(Android.Resource.Id.Text2);

                descriptionText.SetMaxLines(1);
                descriptionText.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

                holder = new ViewHolder { Title = titleText, Description = descriptionText };
                layout.Tag = holder;
                view = layout;
            }
            else
            {
                holder = (ViewHolder)view.Tag;
            }

            holder.Title.Text = movie.Title;
            holder.Description.Text = movie.Description;

            return view;
        }

        internal class ViewHolder : Java.Lang.Object
        {
            public TextView Title { get; set; }
            public TextView Description { get; set; }
        }
    }

}

