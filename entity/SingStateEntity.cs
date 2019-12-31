
using System;
using System.Collections.Generic;
using System.IO;



[Serializable]
public class StatusListItem
{
    /// <summary>
    /// 
    /// </summary>
    public int amount;
    /// <summary>
    /// 
    /// </summary>
    public int period;
    /// <summary>
    /// 
    /// </summary>
    public int status;
    /// <summary>
    /// 
    /// </summary>
    public string time;
}
[Serializable]
public class Data
{
        /// <summary>
        /// 
        /// </summary>
        public int activity;
	/// 
	/// </summary>
	public int isReceive;
        /// 
        /// </summary>
        public List<StatusListItem> statusList;
        /// <summary>
        /// 
        /// </summary>
        public int timing;
}

[Serializable]
public class SingStateEntity
{
        /// <summary>
        /// 
        /// </summary>
        public int code;
        /// <summary>
        /// 
        /// </summary>
        public string message;
        /// <summary>
        /// 
        /// </summary>
        public Data data;
}