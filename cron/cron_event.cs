using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interception.cron {
    public delegate void on_cron_event_executed_callback();
    
    public class cron_event {
        public string name { get; private set; }
        public DateTime execution_time { get; set; }

        Action<object[]> callback;
        object[] parameters;

        public on_cron_event_executed_callback on_cron_event_executed;

        public cron_event(string name, DateTime execution_time, Action<object[]> callback, params object[] parameters) {
            this.name = name;
            this.execution_time = execution_time;
            this.callback = callback;
            this.parameters = parameters;
            this.on_cron_event_executed = delegate () { };
        }

        public void execute() {
            callback(parameters);
            if (on_cron_event_executed != null)
                on_cron_event_executed();
            cron_manager.trigger_on_cron_event_executed_global(this);
        }
    }
}
