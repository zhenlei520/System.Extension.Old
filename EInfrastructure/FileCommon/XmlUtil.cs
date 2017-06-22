using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace EInfrastructure.FileCommon
{
    /// <summary>
    /// 包含对XML操作的函数集合
    /// </summary>
    public class XmlUtil
    {
        #region Methods
        #region 返回XML的dataset表示形式
        /// <summary>
        /// 返回XML的dataset表示形式
        /// </summary>
        /// <param name="path">XML文档路径</param>
        /// <returns></returns>
        public static DataSet GetDataSet(string path)
        {
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(path);
            }
            catch
            {
                // ignored
            }
            return ds;
        }
        #endregion

        #region 获取指定条件的节点
        /// <summary>
        /// 获取指定条件的节点
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="filter">条件</param>
        /// <param name="order">排序方式(可选)</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string path, string filter, string order)
        {
            DataSet ds = GetDataSet(path);
            DataView view = new DataView(ds.Tables[0]);
            if (!string.IsNullOrEmpty(order)) view.RowFilter = filter;
            if (!string.IsNullOrEmpty(order)) view.Sort = order;

            return view.ToTable();
        }
        #endregion

        #region 为XML文档追加项/row
        /// <summary>
        /// 为XML文档追加项/row
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="values">每一列值的组合</param>
        /// <returns></returns>
        public static DataSet AppendItem(string path, string[] values)
        {
            DataSet ds = GetDataSet(path);
            DataTable dt = ds.Tables[0];
            DataRow row = dt.NewRow();
            int cols = Math.Min(dt.Columns.Count, values.Length);
            for (int i = 0; i < cols; i++)
            {
                row[i] = values[i];
            }
            dt.Rows.Add(row);
            dt.AcceptChanges();
            ds.AcceptChanges();
            ds.WriteXml(path);

            return ds;
        }
        #endregion

        #region 在根节点下添加一个节点
        /// <summary>
        /// 在根节点下添加一个节点
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parentNode"></param>
        public static void AddParentNode(string path, string parentNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlElement node = doc.CreateElement(parentNode);
            if (doc.DocumentElement != null) doc.DocumentElement.AppendChild(node);

            doc.Save(path);
        }
        #endregion

        #region 插入一个节点
        /// <summary>
        /// 插入一个节点
        /// </summary>
        /// <param name="path">XML文档路径</param>
        /// <param name="currNode">当前节点xpath</param>
        /// <param name="newElement">新节点名称</param>
        /// <param name="content">新节点内容</param>
        public static void InsertElement(string path, string currNode, string newElement, string content)
        {
            InsertElement(path, currNode, newElement, null, null, content);
        }
        #endregion

        #region 插入一个节点(带属性)
        /// <summary>
        /// 插入一个节点(带属性)
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="currNode">当前节点xpath</param>
        /// <param name="newElement">新节点名</param>
        /// <param name="attName">属性名</param>
        /// <param name="attValue">属性值</param>
        /// <param name="content">新节点内容</param>
        public static void InsertElement(string path, string currNode, string newElement, string attName, string attValue, string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode(currNode);
            XmlElement element = doc.CreateElement(newElement);

            if (!string.IsNullOrEmpty(attName)) element.SetAttribute(attName, attValue);
            element.InnerText = content;

            if (node != null) node.AppendChild(element);

            doc.Save(path);
        }
        #endregion

        #region 插入一个节点(带属性组)

        /// <summary>
        /// 插入一个节点(带属性组)
        /// </summary>
        /// <param name="path">文档路径</param>
        /// <param name="currNode">当前节点xpath</param>
        /// <param name="newElement">新节点名</param>
        /// <param name="attDic"></param>
        /// <param name="content">新节点内容</param>
        public static void InsertElement(string path, string currNode, string newElement, Dictionary<string, string> attDic, string content)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode node = doc.SelectSingleNode(currNode);
            XmlElement element = doc.CreateElement(newElement);

            foreach (var k in attDic.Keys)
            {
                element.SetAttribute(k, attDic[k]);
            }
            element.InnerText = content;

            if (node != null) node.AppendChild(element);

            doc.Save(path);
        }
        #endregion

        #region 向一个节点添加属性
        /// <summary>
        /// 向一个节点添加属性
        /// </summary>
        /// <param name="path">XML文档路径</param>
        /// <param name="currNode">当前节点xpath</param>
        /// <param name="attName">属性名称</param>
        /// <param name="attValue">属性值</param>
        public static void AddAttribute(string path, string currNode, string attName, string attValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlAttribute att = doc.CreateAttribute(attName);
            att.Value = attValue;

            XmlNode node = doc.SelectSingleNode(currNode);
            if (node != null) if (node.Attributes != null) node.Attributes.Append(att);

            doc.Save(path);
        }
        #endregion

        #region 删除一个节点
        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="path">XML文档路径</param>
        /// <param name="node">要删除的节点xpath</param>
        public static void RemoveNode(string path, string node)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //string mainNode = node.Substring(0, node.LastIndexOf("/"));
            //doc.SelectSingleNode(mainNode).RemoveChild(doc.SelectSingleNode(node));
            XmlNode n = doc.SelectSingleNode(node);
            if (n != null) if (n.ParentNode != null) n.ParentNode.RemoveChild(n);

            doc.Save(path);
        }
        #endregion

        #region 删除一个节点属性
        /// <summary>
        /// 删除一个节点属性
        /// </summary>
        /// <param name="path">XML文档路径</param>
        /// <param name="currNode">属性所在节点xpath</param>
        /// <param name="attName">属性名</param>
        public static void RemoveAttribute(string path, string currNode, string attName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlElement xe = (XmlElement)doc.SelectSingleNode(currNode);
            if (xe != null) xe.RemoveAttribute(attName);

            doc.Save(path);

        }
        #endregion

        #region XML文档节点查询和读取
        /**/
        /// <summary>
        /// 选择匹配XPath表达式的第一个节点XmlNode.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNode</returns>
        public static XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(xmlFileName); //加载XML文档
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
                return xmlNode;
            }
            catch
            {
                return null;
                //throw ex; //这里可以定义你自己的异常处理
            }
        }

        /**/
        /// <summary>
        /// 选择匹配XPath表达式的节点列表XmlNodeList.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名")</param>
        /// <returns>返回XmlNodeList</returns>
        public static XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(xmlFileName); //加载XML文档
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                return xmlNodeList;
            }
            catch
            {
                return null;
                //throw ex; //这里可以定义你自己的异常处理
            }
        }

        /**/
        /// <summary>
        /// 选择匹配XPath表达式的第一个节点的匹配xmlAttributeName的属性XmlAttribute.
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <returns>返回xmlAttributeName</returns>
        public static XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlAttribute xmlAttribute = null;
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                if (xmlNode.Attributes != null && xmlNode.Attributes.Count > 0)
                {
                    xmlAttribute = xmlNode.Attributes[xmlAttributeName];
                }
            }
            return xmlAttribute;
        }
        #endregion

        #region XML文档创建和节点或属性的添加、修改
        /**/
        /// <summary>
        /// 创建一个XML文档
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="rootNodeName">XML文档根节点名称(须指定一个根节点名称)</param>
        /// <param name="version">XML文档版本号(必须为:"1.0")</param>
        /// <param name="encoding">XML文档编码方式</param>
        /// <param name="standalone">该值必须是"yes"或"no",如果为null,Save方法不在XML声明上写出独立属性</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateXmlDocument(string xmlFileName, string rootNodeName, string version, string encoding, string standalone)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration(version, encoding, standalone);
            XmlNode root = xmlDoc.CreateElement(rootNodeName);
            xmlDoc.AppendChild(xmlDeclaration);
            xmlDoc.AppendChild(root);
            xmlDoc.Save(xmlFileName);
            return true;
        }

        /**/

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建它的子节点(如果此节点已存在则追加一个新的同名节点
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <param name="value"></param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText, string xmlAttributeName, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //存不存在此节点都创建
                XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                subElement.InnerXml = innerText;

                //如果属性和值参数都不为空则在此新节点上新增属性
                if (!string.IsNullOrEmpty(xmlAttributeName) && !string.IsNullOrEmpty(value))
                {
                    XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                    xmlAttribute.Value = value;
                    subElement.Attributes.Append(xmlAttribute);
                }

                xmlNode.AppendChild(subElement);
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
        }

        /**/
        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建或更新它的子节点(如果节点存在则更新,不存在则创建)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlNodeName">要匹配xmlNodeName的节点名称</param>
        /// <param name="innerText">节点文本值</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateOrUpdateXmlNodeByXPath(string xmlFileName, string xpath, string xmlNodeName, string innerText)
        {
            bool isExistsNode = false;//标识节点是否存在
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //遍历xpath节点下的所有子节点
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Name.ToLower() == xmlNodeName.ToLower())
                    {
                        //存在此节点则更新
                        node.InnerXml = innerText;
                        isExistsNode = true;
                        break;
                    }
                }
                if (!isExistsNode)
                {
                    //不存在此节点则创建
                    XmlElement subElement = xmlDoc.CreateElement(xmlNodeName);
                    subElement.InnerXml = innerText;
                    xmlNode.AppendChild(subElement);
                }
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
        }

        /**/

        /// <summary>
        /// 依据匹配XPath表达式的第一个节点来创建或更新它的属性(如果属性存在则更新,不存在则创建)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要匹配xmlAttributeName的属性名称</param>
        /// <param name="value"></param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool CreateOrUpdateXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName, string value)
        {
            bool isExistsAttribute = false;//标识属性是否存在
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //遍历xpath节点中的所有属性
                if (xmlNode.Attributes != null)
                {
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //节点中存在此属性则更新
                            attribute.Value = value;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (!isExistsAttribute)
                    {
                        //节点中不存在此属性则创建
                        XmlAttribute xmlAttribute = xmlDoc.CreateAttribute(xmlAttributeName);
                        xmlAttribute.Value = value;
                        xmlNode.Attributes.Append(xmlAttribute);
                    }
                }
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
        }
        #endregion

        #region XML文档节点或属性的删除
        /**/
        /// <summary>
        /// 删除匹配XPath表达式的第一个节点(节点中的子元素同时会被删除)
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DeleteXmlNodeByXPath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //删除节点
                if (xmlNode.ParentNode != null) xmlNode.ParentNode.RemoveChild(xmlNode);
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
        }

        /**/
        /// <summary>
        /// 删除匹配XPath表达式的第一个节点中的匹配参数xmlAttributeName的属性
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <param name="xmlAttributeName">要删除的xmlAttributeName的属性名称</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DeleteXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName)
        {
            bool isExistsAttribute = false;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            XmlAttribute xmlAttribute = null;
            if (xmlNode != null)
            {
                //遍历xpath节点中的所有属性
                if (xmlNode.Attributes != null)
                {
                    foreach (XmlAttribute attribute in xmlNode.Attributes)
                    {
                        if (attribute.Name.ToLower() == xmlAttributeName.ToLower())
                        {
                            //节点中存在此属性
                            xmlAttribute = attribute;
                            isExistsAttribute = true;
                            break;
                        }
                    }
                    if (isExistsAttribute)
                    {
                        //删除节点中的属性
                        xmlNode.Attributes.Remove(xmlAttribute);
                    }
                }
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
        }

        /**/
        /// <summary>
        /// 删除匹配XPath表达式的第一个节点中的所有属性
        /// </summary>
        /// <param name="xmlFileName">XML文档完全文件名(包含物理路径)</param>
        /// <param name="xpath">要匹配的XPath表达式(例如:"//节点名//子节点名</param>
        /// <returns>成功返回true,失败返回false</returns>
        public static bool DeleteAllXmlAttributeByXPath(string xmlFileName, string xpath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFileName); //加载XML文档
            XmlNode xmlNode = xmlDoc.SelectSingleNode(xpath);
            if (xmlNode != null)
            {
                //遍历xpath节点中的所有属性
                if (xmlNode.Attributes != null) xmlNode.Attributes.RemoveAll();
            }
            xmlDoc.Save(xmlFileName); //保存到XML文档
            return true;
        }
        #endregion

        #region linqtoxml
        /// <summary>
        /// 创建xml存在则返回
        /// </summary>
        /// <param name="path">xml路径</param>
        /// <param name="root">xml跟节点</param>
        /// <returns></returns>
        public static bool CreateDocument(string path, string root)
        {
            try
            {
                XDocument xdoc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement(root)
                    );
                if (File.Exists(path))
                {
                    return true;
                }
                else
                {
                    xdoc.Save(path);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #endregion
    }
}


