using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using MyBackuper.Classes;

namespace MyBackuper.Service
{
	public partial class MyBackuperService : ServiceBase
	{
		Timer timer = null;
		Backuper backuper = null;

		public MyBackuperService()
		{
			InitializeComponent();

			timer = new Timer(3600000000); // interval - 1 hour
			timer.Elapsed += Timer_Elapsed;

			backuper = Backuper.FromConfig();
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			foreach (var item in backuper)
			{
				if (item.Value.Trigger == BackupTrigger.EveryHour)
				{
					item.Value.MakeBackup();
				}
			}
		}

		protected override void OnStart(string[] args)
		{
			foreach (var item in backuper)
			{
				switch (item.Value.Trigger)
				{
					case BackupTrigger.EveryHour:
						timer.Start();
						break;
					case BackupTrigger.EveryDay:
						item.Value.MakeBackup();
						break;
					case BackupTrigger.EveryWeek:
						if (item.Value.DaysPassed++ == 7)
						{
							item.Value.MakeBackup();
							item.Value.DaysPassed = 0;
						}
						break;
				}
			}
		}

		protected override void OnStop()
		{
			timer.Stop();
		}

		private void WriteLog(string message)
		{
			WriteLog(message, EventLogEntryType.Information);
		}

		private void WriteLog(string message, EventLogEntryType type)
		{
			EventLog.WriteEntry(Properties.Resources.ServiceName, message, type);
		}
	}
}