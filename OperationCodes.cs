namespace Coolector.Common
{
    public static class OperationCodes
    {
        public static string Success => "success";
        public static string InvalidCredentials => "invalid_credentials";
        public static string EmailInUse => "email_in_use";
        public static string NameInUse => "name_in_use";
        public static string InvalidName => "invalid_name";
        public static string InvalidEmail => "invalid_email";
        public static string InvalidPassword => "invalid_password";
        public static string InvalidFile => "invalid_file";
        public static string InvalidCurrentPassword => "invalid_current_password";
        public static string EmailNotFound => "email_not_found";
        public static string InvalidPasswordResetToken => "invalid_password_reset_token";
        public static string FileTooBig => "file_too_big";
        public static string TextTooLong => "text_too_long";
        public static string Error => "error";
    }
}