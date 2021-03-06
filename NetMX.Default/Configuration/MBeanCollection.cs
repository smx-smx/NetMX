#region Using
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

#endregion

namespace NetMX.Server.Configuration
{
   /// <summary>
   /// Represents a collection of MBeans to be created on server start-up.
   /// </summary>
   public class MBeanCollection : ConfigurationElementCollection
   {
      protected override ConfigurationElement CreateNewElement()
      {
         return new MBeanElement();
      }
      protected override object GetElementKey(ConfigurationElement element)
      {
         return ((MBeanElement)element).ObjectName;
      }
   }
}