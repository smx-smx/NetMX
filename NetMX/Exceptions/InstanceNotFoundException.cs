#region USING
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;
#endregion

namespace NetMX
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors"), Serializable]//Other constructos do not make sense.
	public sealed class InstanceNotFoundException : OperationsException
	{
		string _objectName;
		/// <summary>
		/// ObjectName of missing MBean.
		/// </summary>
		public string ObjectName
		{
			get { return _objectName; }
		}
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="objectName">ObjectName of missing MBean.</param>
		public InstanceNotFoundException(string objectName)
			: base(string.Format(CultureInfo.CurrentCulture, "MBean of name \"{0}\" does not exist in this MBeanServer.", objectName))
		{
			_objectName = objectName;
		}

		private InstanceNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{			
			_objectName = info.GetString("objectName");
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods"), System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);			
			info.AddValue("objectName", _objectName);
		}
	}
}
