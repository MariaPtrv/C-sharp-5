using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Lab5_
{
    public enum eFamily { Индоевропейская, Сино_тибетская, Урало_алтайская, Nan };
  

    [Serializable]
    public class Language
    {

        private string name;
        [DisplayName("Название"), Category("Информация о языке")]
        [Description("Название языка")]
        [System.Xml.Serialization.XmlElement("Name")]
        public string Name {
            get; set;
        }
        [DisplayName("Семья"), Category("Информация о языке")]
        [Description("Семья — это группа определённо, но достаточно далеко родственных языков")]
        [System.Xml.Serialization.XmlElement("Family")]
        public eFamily Family { get; set; }
        [DisplayName("Статус"), Category("Информация о языке")]
        [Description("Уровень угрозы языку")]
        [System.Xml.Serialization.XmlElement("Status")]
        public string Status { get; set; }
        [DisplayName("Численность, млн"), Category("Информация о языке")]
        [Description("Число говорящих на языке")]
        [System.Xml.Serialization.XmlElement("Speakers")]
        public int Speakers { get; set; }
        [System.Xml.Serialization.XmlElement("Img")]
        [DisplayName("Фото"), Category("Информация о языке")]
        public string Img { get; set; } = "../../img/default.png";
        [Browsable(false)]
        [System.Xml.Serialization.XmlElement("StrFamily")]
        public string StrFamily
        {
            get
            {
                return Family.ToString();
            }
            set
            {
                Family.ToString();
            }
        }
      


        public Language(string name, eFamily family, string status, int speakers, string img)
        {
            if (string.IsNullOrEmpty(name)) Name = "Underfinded";
            else Name = name;
            if (family.ToString() != " Индоевропейская" && family.ToString() != "Сино_тибетская" && family.ToString() != "Урало_алтайская" && family.ToString() != "Nan")
            Family = eFamily.Nan;
            else Family = family;
            Status = status;
            Speakers = speakers;
            Img = img;
        }
        
        public Language(string name, string family, string status, int speakers, string img)
        {
            if (string.IsNullOrEmpty(name)) Name = "Underfinded";
            else Name = name;
            if (family != "Индоевропейская" && family != "Сино_тибетская" && family != "Урало_алтайская" && family.ToString() != "Nan")
                Family = eFamily.Nan;
            else
            {
                if (family == "Индоевропейская") Family = eFamily.Индоевропейская;
                if (family == "Сино_тибетская") Family = eFamily.Сино_тибетская;
                if (family == "Урало_алтайская") Family = eFamily.Урало_алтайская;
                if (family == "Nan") Family = eFamily.Nan;
            }
            Status = status;
            Speakers = speakers;
            Img = img;
        }
        public Language() { }
    }
}
