﻿//
// Copyright Fela Ameghino 2015-2023
//
// Distributed under the GNU General Public License v3.0. (See accompanying
// file LICENSE or copy at https://www.gnu.org/licenses/gpl-3.0.txt)
//
using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Telegram.Controls
{
    public class SettingsPanel : Panel
    {
        public SettingsPanel()
        {
            ChildrenTransitions = new TransitionCollection
            {
                new RepositionThemeTransition()
            };
        }

        public bool IsHeader { get; set; }
        public bool IsFooter { get; set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            var accumulated = IsFooter ? 0 : 64d;
            var width = Math.Min(availableSize.Width, 1048);

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);

                if (child.DesiredSize.Height > 0)
                {
                    accumulated += child.DesiredSize.Height;
                    accumulated += 16;
                }
            }

            if (IsHeader)
            {
                return new Size(width, accumulated - 16);
            }
            else
            {
                return new Size(width, accumulated + 16);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var accumulated = IsFooter ? 0 : 64d;
            var width = Math.Min(finalSize.Width, 1048);

            foreach (var child in Children)
            {
                child.Arrange(new Rect(0, accumulated, width, child.DesiredSize.Height));

                if (child.DesiredSize.Height > 0)
                {
                    accumulated += child.DesiredSize.Height;
                    accumulated += 16;
                }
            }

            return finalSize;
        }
    }
}
