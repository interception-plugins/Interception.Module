using System;

using SDG.NetTransport;

namespace interception.ui {
    public delegate void on_button_collection_click_callback(int index);

    public sealed class button_collection : collection {
        public on_button_collection_click_callback on_element_click;

        public button_collection(control _parent, short _key, ITransportConnection _tc, string _name, string _suffix_format, int _max_elements) 
            : base(_parent, _key, _tc, _name, _suffix_format, _max_elements) {

        }

        internal void element_click(int index) {
            if (on_element_click != null)
                on_element_click(index);
            ui_manager.trigger_on_button_collection_click_global(index, this);
        }
    }
}
