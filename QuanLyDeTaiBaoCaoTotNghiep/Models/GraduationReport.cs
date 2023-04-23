﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyDeTaiBaoCaoTotNghiep.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class GraduationReport
    {
        public int GraduationReportID { get; set; }
        [DisplayName("Tên báo cáo")]
        public string GraduationReportName { get; set; }
        [DisplayName("Hình ảnh")]
        public string Image { get; set; }
        [DisplayName("Tên lớp")]
        public int ClassID { get; set; }
        [DisplayName("Tên khoa viện")]
        public int FacultyID { get; set; }
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        public Nullable<int> ID { get; set; }
        [DisplayName("Tác giả")]
        public string Author { get; set; }
        public Nullable<int> CommentID { get; set; }
        public string UrlFile { get; set; }
        [DisplayName("Từ khóa")]
        public string Keyword { get; set; }
        [DisplayName("Niên khóa")]
        public Nullable<int> YearID { get; set; }
        [DisplayName("Ngày tải lên")]
        public Nullable<System.DateTime> UploadDate { get; set; }
        [DisplayName("Niên khóa")]
        public virtual AcademicYear AcademicYear { get; set; }
        [DisplayName("Tên lớp")]
        public virtual Class Class { get; set; }
        [DisplayName("Tên khoa viện")]
        public virtual Faculty Faculty { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public bool Status { get; set; }
    
       
    }
}
