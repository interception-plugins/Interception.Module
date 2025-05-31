using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SDG.NetTransport;

namespace interception.ui {
    public abstract class control {
        public abstract control parent { get; }
        protected abstract short key { get; }
        protected abstract ITransportConnection tc { get; }
        public abstract string name { get; }
        // todo
        //public abstract bool is_visible { get; }
        public abstract void show();
        public abstract void hide();
    }
}
