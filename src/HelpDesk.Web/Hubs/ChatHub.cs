using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HelpDesk.Web.Hubs
{
	public class ChatHub : Hub
	{
        public async Task Enter(string username, string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task Send(string message, string userName, string groupName)
        {
            await Clients.Group(groupName).SendAsync("Receive", message, userName);
        }
	}
}

