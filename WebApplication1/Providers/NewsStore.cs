using WebApplication1.SignalRHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using Solo.Base.PLC.PLC_Entities;
using Konnexis.Runtime.Remoting;

namespace WebApplication1.Providers
{
    public class NewsStore
    {
        Dictionary<string, List<NewsItem>> _groups;
        string _channelName = Guid.NewGuid().ToString();
        string _remoting_Host = "10.198.100.23";
        string _remoting_Port = "6554";
        string ServiceName = "SoloRemotingServer";

        IRemotingServer _plcRemotingServer;

        public NewsStore()
        {
            RegisterChannel();

            _plcRemotingServer = (IRemotingServer)Activator.CreateInstance(typeof(IRemotingServer),
                "http://" + _remoting_Host + ":" + _remoting_Port + "/" + ServiceName);

            _plcRemotingServer.SenseServer();
        }

        //private readonly NewsContext _newsContext;

        public void AddGroup(string group)
        {
            _groups.Add(group, null);
            //_newsContext.NewsGroups.Add(new NewsGroup
            //{
            //    Name = group
            //});
            //_newsContext.SaveChanges();
        }

        public bool GroupExists(string group)
        {
            //var item = _newsContext.NewsGroups.FirstOrDefault(t => t.Name == group);
            //if (item == null)
            //{
            //    return false;
            //}

            return _groups.Keys.Contains(group);
        }

        public void CreateNewItem(NewsItem item)
        {
            if (GroupExists(item.NewsGroup))
            {
                _groups[item.NewsGroup].Add(new NewsItem()
                {
                    Author = "Author" + item.NewsGroup,
                    Header = "Header" + item.NewsGroup,
                    NewsGroup = "NewsGroup" + item.NewsGroup,
                    NewsText = "NewsText" + item.NewsGroup
                });
            }
            else
            {
                throw new System.Exception("group does not exist");
            }
        }

        public IEnumerable<NewsItem> GetAllNewsItems(string group)
        {
            return _groups[group];
        }

        public List<string> GetAllGroups()
        {
            //return _newsContext.NewsGroups.Select(t => t.Name).ToList();
            return _groups.Keys.ToList();
        }

        private void RegisterChannel()
        {
            try
            {
                ChannelManager.RegisterClientChannel(0, System.Runtime.Serialization.Formatters.TypeFilterLevel.Full, _channelName);
            }
            catch (Exception ex)
            {
                //Helper.ReportLayerException(ex);
            }
        }

        private void UnRegisterChannel()
        {
            try
            {
                ChannelManager.UnregisterChannel(_channelName);
            }
            catch (Exception ex)
            {
                //Helper.ReportLayerException(ex);
            }
        }
    }
}
