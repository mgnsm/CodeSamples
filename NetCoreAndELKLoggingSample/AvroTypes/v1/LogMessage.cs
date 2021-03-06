// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen, version 1.7.7.5
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace AvroTypes.v1
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using global::Avro;
	using global::Avro.Specific;
	
	public partial class LogMessage : ISpecificRecord
	{
		public static Schema _SCHEMA = Schema.Parse(@"{""type"":""record"",""name"":""LogMessage"",""namespace"":""AvroTypes.v1"",""fields"":[{""name"":""LogLevel"",""type"":{""type"":""enum"",""name"":""LogLevel"",""namespace"":""AvroTypes.v1"",""symbols"":[""Trace"",""Debug"",""Information"",""Warning"",""Error"",""Critical"",""None""]}},{""name"":""EventId"",""type"":[""null"",{""type"":""record"",""name"":""EventId"",""namespace"":""AvroTypes.v1"",""fields"":[{""name"":""Id"",""type"":""int""},{""name"":""Name"",""type"":[""null"",""string""]}]}]},{""name"":""Exception"",""type"":[""null"",{""type"":""record"",""name"":""Exception"",""namespace"":""AvroTypes.v1"",""fields"":[{""name"":""StackTrace"",""type"":[""null"",""string""]},{""name"":""Source"",""type"":[""null"",""string""]},{""name"":""Message"",""type"":[""null"",""string""]},{""name"":""HResult"",""type"":""int""},{""name"":""HelpLink"",""type"":[""null"",""string""]}]}]},{""name"":""Message"",""type"":[""null"",""string""]},{""name"":""Timestamp"",""type"":""long""}]}");
		private AvroTypes.v1.LogLevel _LogLevel;
		private AvroTypes.v1.EventId _EventId;
		private AvroTypes.v1.Exception _Exception;
		private string _Message;
		private long _Timestamp;
		public virtual Schema Schema
		{
			get
			{
				return LogMessage._SCHEMA;
			}
		}
		public AvroTypes.v1.LogLevel LogLevel
		{
			get
			{
				return this._LogLevel;
			}
			set
			{
				this._LogLevel = value;
			}
		}
		public AvroTypes.v1.EventId EventId
		{
			get
			{
				return this._EventId;
			}
			set
			{
				this._EventId = value;
			}
		}
		public AvroTypes.v1.Exception Exception
		{
			get
			{
				return this._Exception;
			}
			set
			{
				this._Exception = value;
			}
		}
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message = value;
			}
		}
		public long Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.LogLevel;
			case 1: return this.EventId;
			case 2: return this.Exception;
			case 3: return this.Message;
			case 4: return this.Timestamp;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.LogLevel = (AvroTypes.v1.LogLevel)fieldValue; break;
			case 1: this.EventId = (AvroTypes.v1.EventId)fieldValue; break;
			case 2: this.Exception = (AvroTypes.v1.Exception)fieldValue; break;
			case 3: this.Message = (System.String)fieldValue; break;
			case 4: this.Timestamp = (System.Int64)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
