
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace APIDL
{


    /// <summary>
    /// Static Factory class for creating Dal tier implementation object according to
    /// configuration in file config.xml
    /// </summary>
    public static class DalFactory
    {
        /// <summary>
        /// The function creates Dal tier implementation object according to Dal type
        /// as appears in "dal" element in the configuration file config.xml.<br/>
        /// The configuration file also includes element "dal-packages" with list
        /// of available packages (.dll files) per Dal type.<br/>
        /// Each Dal package must use "Dal" namespace and it must include internal access
        /// singleton class with the same name as package's name.<br/>
        /// The singleton class must include public static property called "Instance"
        /// which must contain the single instance of the class.
        /// </summary>
        /// <returns>Dal tier implementation object</returns>
        public static IDAL GetDal()
        {
            // get dal implementation name from config.xml according to <data> element
            string dalType = DalConfig.DalName;
            //string dalType ="DalObject";
            // bring package name (dll file name) for the dal name (above) from the list of packages in config.xml
            string dalPackage = DalConfig.DalPackages[dalType];
            // if package name is not found in the list - there is a problem in config.xml
            if (dalPackage == null)
                throw new DalConfigException($"Wrong DL type: {dalType}");

            try // Load into CLR the dal implementation assembly according to dll file name (taken above)
            {
                Assembly.Load(dalPackage);
            }
            catch (Exception ex)
            {
                throw new DalConfigException($"Failed loading {dalPackage}.dll", ex);
            }

            // Get concrete Dal implementation's class metadata object
            // 1st element in the list inside the string is full class name: namespace="Dal", class name = package name
            //    the last requirement (class name = package name) is not mandatory in general - but this is the way it
            //    is configured per the implementation here, otherwise we'd need to add class name in addition to package
            //    name in the config.xml file - which is clearly a good option.
            //    NB: the class may not be public - it will still be found... Our approach that the implemntation class
            //        should hold "internal" access permission (which is actually the default access permission)
            // 2nd element is the package name = assembly name (as above)
            Type type = Type.GetType($"DL.{dalPackage}, {dalPackage}");
            // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
            if (type == null)
                throw new DalConfigException($"Class name is not the same as Assembly Name: {dalPackage}");

            // *** Get concrete Dal implementation's Instance
            // Get property info for public static property named "Instance" (in the dal implementation class- taken above)
            // If the property is not found or it's not public or not static then it is not properly implemented
            // as a Singleton...
            // Get the value of the property Instance (get function is automatically called by the system)
            // Since the property is static - the object parameter is irrelevant for the GetValue() function and we can use null
            try
            {
                IDAL dal = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) as IDAL;
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                if (dal == null)
                    throw new DalConfigException($"Class {dalPackage} instance is not initialized");
                // now it looks like we have appropriate dal implementation instance :-)
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DalConfigException($"Class {dalPackage} is not a singleton", ex);
            }

        }
    }
}