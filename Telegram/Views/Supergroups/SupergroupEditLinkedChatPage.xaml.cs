//
// Copyright Fela Ameghino 2015-2023
//
// Distributed under the GNU General Public License v3.0. (See accompanying
// file LICENSE or copy at https://www.gnu.org/licenses/gpl-3.0.txt)
//
using Microsoft.UI.Xaml.Controls;
using Telegram.Controls.Cells;
using Telegram.Td.Api;
using Telegram.ViewModels.Delegates;
using Telegram.ViewModels.Supergroups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Telegram.Views.Supergroups
{
    // TODO: Convert to TableListView
    public sealed partial class SupergroupEditLinkedChatPage : HostedPage, ISupergroupDelegate
    {
        public SupergroupEditLinkedChatViewModel ViewModel => DataContext as SupergroupEditLinkedChatViewModel;

        public SupergroupEditLinkedChatPage()
        {
            InitializeComponent();
            Title = Strings.Discussion;
        }

        private void OnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            var button = args.Element as Button;
            var content = button.Content as UserCell;

            var chat = button.DataContext as Chat;

            content.UpdateLinkedChat(ViewModel.ClientService, sender, args);

            button.CommandParameter = chat;
            button.Command = ViewModel.LinkCommand;
        }

        #region Delegate

        public void UpdateSupergroup(Chat chat, Supergroup group)
        {
            Headline.Text = string.Format(Strings.DiscussionChannelGroupSetHelp2, chat.Title);

            Create.Visibility = group.HasLinkedChat ? Visibility.Collapsed : Visibility.Visible;
            Unlink.Visibility = group.HasLinkedChat ? Visibility.Visible : Visibility.Collapsed;
            Unlink.Content = group.IsChannel ? Strings.DiscussionUnlinkGroup : Strings.DiscussionUnlinkChannel;
        }

        public void UpdateSupergroupFullInfo(Chat chat, Supergroup group, SupergroupFullInfo fullInfo)
        {
            var linkedChat = ViewModel.ClientService.GetChat(fullInfo.LinkedChatId);
            if (linkedChat != null)
            {
                if (group.IsChannel)
                {
                    Headline.Text = string.Format(Strings.DiscussionChannelGroupSetHelp2, linkedChat.Title);
                    LayoutRoot.Footer = Strings.DiscussionChannelHelp2;
                }
                else
                {
                    Headline.Text = string.Format(Strings.DiscussionGroupHelp, linkedChat.Title);
                    LayoutRoot.Footer = Strings.DiscussionChannelHelp2;
                }

                JoinToSendMessages.Visibility = group.IsChannel ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                JoinToSendMessages.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateChat(Chat chat) { }
        public void UpdateChatTitle(Chat chat) { }
        public void UpdateChatPhoto(Chat chat) { }

        #endregion

        #region Binding

        private string ConvertJoinToSendMessages(bool joinToSendMessages)
        {
            return joinToSendMessages ? Strings.ChannelSettingsJoinRequestInfo : Strings.ChannelSettingsJoinToSendInfo;
        }

        #endregion
    }
}
