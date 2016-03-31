﻿using PSCredentialManager.Common;
using PSCredentialManager.Common.Enum;
using System;
using System.Runtime.InteropServices;

namespace PSCredentialManager.Api.Utility
{
    public static class CustomExtensions
    {
        public static Credential ToCredential(this NativeCredential nativeCredential)
        {
            Credential credential;

            try
            {
                credential = new Credential()
                {
                    Type = nativeCredential.Type,
                    Flags = nativeCredential.Flags,
                    Persist = (CRED_PERSIST)nativeCredential.Persist,
                    UserName = Marshal.PtrToStringUni(nativeCredential.UserName),
                    TargetName = Marshal.PtrToStringUni(nativeCredential.TargetName),
                    TargetAlias = Marshal.PtrToStringUni(nativeCredential.TargetAlias),
                    Comment = Marshal.PtrToStringUni(nativeCredential.Comment),
                    CredentialBlobSize = nativeCredential.CredentialBlobSize
                };

                if (0 < nativeCredential.CredentialBlobSize)
                {
                    credential.CredentialBlob = Marshal.PtrToStringUni(nativeCredential.CredentialBlob, (int)nativeCredential.CredentialBlobSize / 2);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("PSCredentialManager.Api.CredentialUtility.ConvertToCredential Unable to convert native credential to credential.", ex);
            }

            return credential;
        }

        public static NativeCredential ToNativeCredential(this Credential credential)
        {
            NativeCredential nativeCredential;

            try
            {
                nativeCredential = new NativeCredential()
                {
                    AttributeCount = 0,
                    Attributes = IntPtr.Zero,
                    Comment = Marshal.StringToCoTaskMemUni(credential.Comment),
                    TargetAlias = IntPtr.Zero,
                    Type = credential.Type,
                    Persist = (uint)credential.Persist,
                    CredentialBlobSize = (UInt32)credential.CredentialBlobSize,
                    TargetName = Marshal.StringToCoTaskMemUni(credential.TargetName),
                    CredentialBlob = Marshal.StringToCoTaskMemUni(credential.CredentialBlob),
                    UserName = Marshal.StringToCoTaskMemUni(credential.UserName)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("PSCredentialManager.Api.CredentialUtility.ConvertToNativeCredential Unable to convert credential to native credential.", ex);
            }

            return nativeCredential;
        }
    }
}