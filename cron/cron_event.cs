using System;

namespace interception.cron {
    public delegate void on_cron_event_executed_callback();
    
    public class cron_event {
        public string name { get; private set; }
        public TimeSpan execution_time { get; set; }
        public bool trigger_once { get; set; }

        readonly Action<object[]> callback;
        readonly object[] parameters;

        public on_cron_event_executed_callback on_cron_event_executed;

        public cron_event(string name, TimeSpan execution_time, bool trigger_once, Action<object[]> callback, params object[] parameters) {
            this.name = name;
            this.execution_time = execution_time;
            this.trigger_once = trigger_once;
            this.callback = callback;
            this.parameters = parameters;
        }

        public void execute() {
            callback(parameters);
            if (on_cron_event_executed != null)
                on_cron_event_executed();
            cron_manager.trigger_on_cron_event_executed_global(this);
        }
    }
}
