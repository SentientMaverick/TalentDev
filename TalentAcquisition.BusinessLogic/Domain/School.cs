using System;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
namespace TalentAcquisition.Core.Domain
{
    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion

    }

    public enum GradeObtained
    {
        BEng,
        HND,
        OND,
        PGD,
        WASSCE,
        FLSC
    }
    public class School
    {
        public int ID { get; set; }
        public int JobSeekerID { get; set; }
        public string SchoolName { get; set; }
        public GradeObtained Level { get; set; }
        public string CourseOfStudy { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
        //public static string GetEnumDescription(Enum value)
        //{
        //    FieldInfo fi = value.GetType().GetField(value.ToString());

        //    DescriptionAttribute[] attributes =
        //        (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //    if (attributes != null && attributes.Length > 0)
        //        return attributes[0].Description;
        //    else
        //        return value.ToString();
        //}
        //public static string GetStringValue(this Enum value)
        //{
        //    // Get the type
        //    Type type = value.GetType();

        //    // Get fieldinfo for this type
        //    FieldInfo fieldInfo = type.GetField(value.ToString());

        //    // Get the stringvalue attributes
        //    StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
        //        typeof(StringValueAttribute), false) as StringValueAttribute[];

        //    // Return the first if there was a match.
        //    return attribs.Length > 0 ? attribs[0].StringValue : null;
        //}
    }
}