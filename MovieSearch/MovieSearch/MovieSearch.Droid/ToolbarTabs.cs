using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MovieSearch.Droid
{
    using Android.Support.Design.Widget;
    using Android.Support.V4.App;
    using Android.Support.V4.View;

    public static class ToolbarTabs
    {
        public static void Construct(FragmentActivity activity, Toolbar toolbar)
        {
            var topRatedFragment = new TopRatedFragment();
            var fragments = new Fragment[]
                                {
                                    new MovieInputFragment(),
                                    topRatedFragment
                                };
            var titles = CharSequence.ArrayFromStringArray(new[]
                                {
                                    "Movie",
                                    "TopRated"
                                });

            var viewPager = activity.FindViewById<ViewPager>(Resource.Id.viewpager);
            viewPager.Adapter = new TabsFragmentPagerAdapter(activity.SupportFragmentManager, fragments, titles);

            // Give the TabLayout the ViewPager
            var tabLayout = activity.FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            tabLayout.SetupWithViewPager(viewPager);

            tabLayout.TabSelected += async (sender, args) =>
            {
                viewPager.Enabled = false;

                var tab = args.Tab;
                if (tab.Position == 1)
                {
                    await topRatedFragment.FetchTopRatedMovies();
                }

                viewPager.Enabled = true;
            };

            SetToolbar(activity, toolbar);
        }

        public static void SetToolbar(Activity activity, Toolbar toolbar)
        {
            activity.SetActionBar(toolbar);
            activity.ActionBar.Title = activity.GetString(Resource.String.ToolbarTitle);
        }
    }
}