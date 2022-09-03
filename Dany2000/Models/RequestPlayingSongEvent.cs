using Dany2000.Api;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dany2000.Models
{
    internal class RequestPlayingSongEvent : PubSubEvent<Song>
    {

    }
}
