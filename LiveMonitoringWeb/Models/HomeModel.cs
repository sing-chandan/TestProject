using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace LiveMonitoringWeb.Models
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DefaultConnection") { }

        public DbSet<UserProfile> tbl_UserProfile { get; set; }

        public DbSet<webpages_Membership> tbl_webpages_Membership { get; set; }

        public DbSet<webpages_Roles> tbl_webpages_Roles { get; set; }

        public DbSet<webpages_UsersInRoles> tbl_webpages_UsersInRoles { get; set; }

        public DbSet<MachineDetail> MachineDetails { get; set; }

        public DbSet<BrowserDetail> BrowserDetails { get; set; }

        public DbSet<KeyLogging> KeyLoggings { get; set; }

        public DbSet<MachineIdle> MachineIdles { get; set; }

        public DbSet<Tree> Trees { get; set; }

        public DbSet<AppDetail> AppDetails { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<SubCategory> SubCategory { get; set; }

        public DbSet<SubCategoryType> SubCategoryType { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<Screens> Screens { get; set; }

        public DbSet<UserScreenPermissions> UserScreenPermissions { get; set; }

        public DbSet<CustomerUserMapping> CustomerUserMapping { get; set; }

        public DbSet<ScheduleReports> ScheduleReports { get; set; }

        public DbSet<CustomerDownloadHistory> CustomerDownloadHistory { get; set; }

        public DbSet<AdminScheduleReport> AdminScheduleReport { get; set; }

        public DbSet<Groups> Groups { get; set; }

        public DbSet<MachineGrouping> MachineGroupings { get; set; }

        public DbSet<MachineSession> MachineSessions { get; set; }
    
    }

    #region Database Models

    [Table("webpages_Membership")]
    public class webpages_Membership
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public string ConfirmationToken { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTime LastPasswordFailureDate { get; set; }

        public int PasswordFailuresSinceLastSuccess { get; set; }

        public string Password { get; set; }

        public DateTime PasswordChangedDate { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordVerificationToken { get; set; }

        public DateTime PasswordVerificationTokenExpirationDate { get; set; }
    }

    [Table("webpages_Roles")]
    public class webpages_Roles
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }

    [Table("webpages_UsersInRoles")]
    public class webpages_UsersInRoles
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }

    [Table("LiveMonitoring_MachineDetails")]
    public class MachineDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MachineDetailId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Machine MacAddress")]
        public string MachineMacAddress { get; set; }

        [Required]
        [Display(Name = "Machine Name")]
        public string MachineName { get; set; }

        [Required]
        [Display(Name = "Machine IP")]
        public string MachineIP { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Block Status")]
        public bool IsBlocked { get; set; }

        public Customer customer { get; set; }

        [NotMapped]
        public string GroupName { get; set; }

        [NotMapped]
        public int GroupId { get; set; }
    }

    [Table("LiveMonitoring_BrowserDetails")]
    public class BrowserDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BrowserDetailId { get; set; }

        public int MachineDetailId { get; set; }

        [Required]
        [Display(Name = "Browser Name")]
        public string BrowserName { get; set; }

        [Required]
        [Display(Name = "Browser Version")]
        public string BrowserVersion { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "URL")]
        public string URL { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public int count { get; set; }

        public MachineDetail machine_detail { get; set; }
    }

    [Table("LiveMonitoring_KeyLoggings")]
    public class KeyLogging
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int KeyLoggerId { get; set; }

        public int MachineDetailId { get; set; }

        [Display(Name = "Text Type")]
        public string TextType { get; set; }

        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public MachineDetail machine_detail { get; set; }

    }

    [Table("LiveMonitoring_Idle")]
    public class MachineIdle
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IdleTimeId { get; set; }

        public int MachineDetailId { get; set; }

        [Display(Name = "Idle Time (sec)")]
        public int IdleTime { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public MachineDetail machine_detail { get; set; }

    }

    [Table("LiveMonitoring_AppDetails")]
    public class AppDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AppId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "App Name")]
        public string AppName { get; set; }

        public int MachineDetailId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public int count { get; set; }

        public MachineDetail machine_detail { get; set; }
    }

    [Table("LiveMonitoring_SubCategoryType")]
    public class SubCategoryType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SubCategoryTypeId { get; set; }

        [Display(Name = "SubCategory Type")]
        public string SubCategoryTypeName { get; set; }

    }

    [Table("LiveMonitoring_Category")]
    public class Category
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        [Display(Name = "Block")]
        public bool IsBlocked { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }

        //public UserProfile UserProfile { get; set; }
    }

    [Table("LiveMonitoring_Configuration")]
    public class Configuration
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ConfigurationId { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Browser")]
        public int BrowserTacker_Interval { get; set; }

        [Display(Name = "ScreenShot")]
        public int ScreenShot_Interval { get; set; }

        [Display(Name = "Machine Idle")]
        public int MachineIdle_Interval { get; set; }

        [Display(Name = "MachineIdle MinTime")]
        public int MachineIdle_MinTime { get; set; }

        [Display(Name = "KeyLogger")]
        public int KeyLogger_Interval { get; set; }

        [Display(Name = "KeyLogger MinTime")]
        public int KeyLogger_MinTime { get; set; }

        [Display(Name = "App Tracker")]
        public int AppTracker_Interval { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Block Data")]
        public bool IsSendBlockData { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "User")]
        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public UserProfile UserProfile { get; set; }

        public Customer customer { get; set; }
    }

    [Table("LiveMonitoring_SubCategory")]
    public class SubCategory
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }

        [Required]
        [Display(Name = "Category Id")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "SubCategoryType Id")]
        public int SubCategoryTypeId { get; set; }

        [Required]
        [Display(Name = "SubCategory Name")]
        public string SubCategoryName { get; set; }

        [Display(Name = "Productive")]
        public bool IsProductive { get; set; }

        [Display(Name = "Block")]
        public bool IsBlocked { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }

        public Category Category { get; set; }

        public SubCategoryType SubCategoryType { get; set; }

    }

    [Table("LiveMonitoring_Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        public int MembershipId { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? LastLoginDate { get; set; }
       
        [NotMapped]
        [Display(Name = "Users")]
        public int count { get; set; }

        [NotMapped]
        [Display(Name = "Downloads")]
        public int Downloads { get; set; }

       

    }

    [Table("LiveMonitoring_Subscribers")]
    public class Subscriber
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SubscriberId { get; set; }

        [Required]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Free Users")]
        public int FreeUsers { get; set; }

        [Required]
        [Display(Name = "Paid Users")]
        public int PaidUsers { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }

        public Customer customer { get; set; }
    }

    [Table("LiveMonitoring_Screens")]
    public class Screens
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ScreenId { get; set; }

        [Required]
        [Display(Name = "Screen Display Name")]
        public string ScreenDisplayName { get; set; }

        [Required]
        [Display(Name = "Screen Internal Name")]
        public string ScreenInternalName { get; set; }

        [Required]
        [Display(Name = "Screen Type")]
        public string ScreenType { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }

        [NotMapped]
        public bool Selected { get; set; }

        [NotMapped]
        public int ScheduleReportId { get; set; }

        [NotMapped]
        public string ScheduleType { get; set; }

        [NotMapped]
        public int UserScreenPermissionId { get; set; }
        [NotMapped]
        public int countRecord { get; set; }

    }

    [Table("LiveMonitoring_UserScreenPermissions")]
    public class UserScreenPermissions
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserScreenPermissionId { get; set; }

        [Required]
        [Display(Name = "Membership Id")]
        public int MembershipId { get; set; }

        [Required]
        [Display(Name = "Screen Id")]
        public int ScreenId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

    }

    [Table("LiveMonitoring_CustomerUserMapping")]
    public class CustomerUserMapping
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerUserId { get; set; }

        [Required]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Membership Id")]
        public int MembershipId { get; set; }

    }

    [Table("LiveMonitoring_ScheduleReports")]
    public class ScheduleReports
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ScheduleReportId { get; set; }

        [Required]
        [Display(Name = "Screen Id")]
        public int ScreenId { get; set; }

        [Required]
        [Display(Name = "Membership Id")]
        public int MembershipId { get; set; }

        [Required]
        [Display(Name = "Schedule Type")]
        public string ScheduleType { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [Display(Name = "Send Date")]
        public DateTime? SendDate { get; set; }

        [Display(Name = "Is Send")]
        public bool? IsSend { get; set; }


    }

    [Table("LiveMonitoring_CustomerDownloadHistory")]
    public class CustomerDownloadHistory
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DownloadHistoryId { get; set; }

        [Required]
        [Display(Name = "CustomerId")]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Download Date")]
        public DateTime DownloadDate { get; set; }

        public Customer customer { get; set; }
       
    }

    [Table("LiveMonitoring_AdminScheduleReport")]
    public class AdminScheduleReport
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AdminScheduleReportId { get; set; }      
        public int CustomerId { get; set; }
        public int MachineDetailId { get; set; }
        public bool bSend { get; set; }
    }

    [Table("LiveMonitoring_Groups")]
    public class Groups
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Required]
        [Display(Name = "Shift Start Time")]
        public string ShiftStartTime { get; set; }

        [Required]
        [Display(Name = "Shift End Time")]
        public string ShiftEndTime { get; set; }

        [Required]
        [Display(Name = "Lunch Start Time")]
        public string LunchStartTime { get; set; }

        [Required]
        [Display(Name = "Lunch End Time")]
        public string LunchEndTime { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }
     

    }

    [Table("LiveMonitoring_MachineGrouping")]
    public class MachineGrouping
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MachineGroupId { get; set; }

        public int MachineDetailId { get; set; }

        public int GroupId { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public MachineDetail machinedetail { get; set; }

        public Groups groups { get; set; }
    }

    [Table("LiveMonitoring_MachineSessions")]
    public class MachineSession
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MachineSessionId { get; set; }

        public int MachineDetailId { get; set; }

        [Display(Name = "Session Start")]
        public DateTime SessionStart { get; set; }

        [Display(Name = "Session End")]
        public DateTime SessionEnd { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? ModifiedDate { get; set; }

        public MachineDetail machinedetail { get; set; }
    }

    #endregion

    #region UI Models

    public class FileList
    {
        public string FileURL { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string CaptureDate { get; set; }
    }


    public class treeWorkingPerformance
    {
        public List<Tree> tree { get; set; }      
    }

    public class WorkingPerformance
    {
        public string dateOfMonth { get; set; }
        public string workingTime { get; set; }
        public string idleTime { get; set; }
        public string groupTime { get; set; }
    }

    public class WorkingPerformancetest
    {
        public int ID { get; set; }
        public int MachineID { get; set; }
        public string EntryDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class Tree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual ICollection<Tree> Group { get; set; }
        [NotMapped]
        public string MachineMacAddress { get; set; }
    }

    public class NewTree
    {
        public int id { get; set; }
        public string text { get; set; }
        public int? parent { get; set; }
        public virtual ICollection<Tree> Group { get; set; }
        [NotMapped]
        public string MachineMacAddress { get; set; }
    }

    public class ScreenShot
    {
        public List<Tree> tree { get; set; }
        public List<FileList> filelist { get; set; }
    }
    public class Selectlist
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class ReturnSchedule
    {
        public List<Selectlist> ScheduleType { get; set; }
        public List<Screens> Screenlist { get; set; }
    }

    public class ReturnSubCategory
    {
        public List<SubCategory> SubCategory{get;set;}
        public List<Category> Categorylist { get; set; }
        public List<SubCategoryType> SubCategoryType { get; set; }
    }

    public class ReturnUserData
    {
        public List<UserProfile> Userlist { get; set; }
        public List<webpages_Roles> UserRoles { get; set; }
    }

    public class ReturnUserPermission
    {
        public List<Screens> ScreenList { get; set; }
        public IList<SelectListItem> Userlist { get; set; }
        public int selectId { get; set; }
    }

    public class ReturnData
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
    }

    public class ProjectStatics
    {
        [Display(Name = "MachineDetail Id")]
        public int MachineDetailId { get; set; }
        [Display(Name = "Machine Name")]
        public string MachineName { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Key Log")]
        public string KeyLoggerCount { get; set; }
        [Display(Name = "Browser Detail")]
        public string BrowserDetailCount { get; set; }
        [Display(Name = "App Detail")]
        public string AppDetailCount { get; set; }
        [Display(Name = "Idle Time(Count)")]
        public string IdleTimeCount { get; set; }
        [Display(Name = "Idle Time")]
        public string IdleTimeSum { get; set; }
    }

    public class ScheduleReportsSettings
    {
        public int ScheduleReportId { get; set; }

        public int ScreenId { get; set; }

        public string ScreenInternalName { get; set; }

        public int MembershipId { get; set; }

        public string ScheduleType { get; set; }

        public int CustomerId { get; set; }

        public string Email { get; set; }

        public DateTime SendDate { get; set; }

        public bool IsSend { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public int countCustomer { get; set; }

    }

    #endregion

    #region ----Web API parameters For POST-----
    public class ScheduleReportsPOSTparam
    {
        public int ScreenId { get; set; }
        public int MembershipId { get; set; }
        public string sSheduleType { get; set; }
    }
    #endregion
}