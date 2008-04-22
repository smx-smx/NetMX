#region Using
using System;
using System.Collections.Generic;
using System.Reflection;
using NetMX;

#endregion

namespace NetMX.OpenMBean
{
   /// <summary>
   /// Describes a constructor of an Open MBean.   
   /// </summary>
   public class OpenMBeanConstructorInfoSupport : MBeanConstructorInfo, IOpenMBeanConstructorInfo
   {
      /// <summary>
      /// Constructs an MBeanConstructorInfo object.
      /// </summary>      
      /// <param name="name">Name of constructor</param>
      /// <param name="description">Description of constructor</param>
      /// <param name="signature">Parameters for this constructor.</param>
      public OpenMBeanConstructorInfoSupport(string name, string description, IEnumerable<IOpenMBeanParameterInfo> signature)
			: base(name, description, OpenInfoUtils.Transform<MBeanParameterInfo, IOpenMBeanParameterInfo>(signature), true)
		{
		}
      /// <summary>
      /// Constructs an MBeanConstructorInfo object.
      /// </summary>
      /// <param name="info">Object describing CLR constructor.</param>      
      public OpenMBeanConstructorInfoSupport(ConstructorInfo info)
			: base(info, true)
		{         
         ParameterInfo[] paramInfos = info.GetParameters();
         List<MBeanParameterInfo> tmp = new List<MBeanParameterInfo>();
         for (int i = 0; i < paramInfos.Length; i++)
         {
            tmp.Add(new OpenMBeanParameterInfoSupport(paramInfos[i]));
         }
         Signature = tmp.AsReadOnly();	
		}
   }
}