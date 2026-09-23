using HarmonyLib;
using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace SNLFWideNumericInterop
{
    /// <summary>
    /// Optional, reflection-only bridge to Cosmo Mod Library's Save n Load Fixes.
    /// Tel mods keep their existing vanilla-compatible arithmetic when SNLF is absent.
    /// When SNLF owns the wide-numeric layer, use its checked/exact helpers so Tel-side
    /// gameplay modifiers do not narrow Int64 values back through Single/Int32.
    /// </summary>
    internal static class SNLFWideNumeric
    {
        internal const string HarmonyId = "com.cosmo.savenloadfixes";

        private const string ContinuationTypeName = "SaveNLoadFixes.Repairs.WideNumericContinuation";
        private const string RepairTypeName = "SaveNLoadFixes.Repairs.WideNumericRepair";

        private static bool resolved;
        private static bool warned;
        private static MethodInfo roundSingleProductCompatible;
        private static MethodInfo add;
        private static MethodInfo multiply;
        private static MethodInfo getAverageEarnings;

        internal static bool IsEnabled
        {
            get { return Harmony.HasAnyPatches(HarmonyId); }
        }

        internal static bool TryRoundSingleProduct(
            long value,
            string context,
            out long result,
            params float[] coefficients)
        {
            result = 0L;
            if (!IsEnabled)
                return false;

            Resolve();
            if (roundSingleProductCompatible == null)
            {
                WarnUnavailable();
                return false;
            }

            try
            {
                result = (long)roundSingleProductCompatible.Invoke(
                    null,
                    new object[] { value, context, coefficients });
                return true;
            }
            catch (TargetInvocationException exception)
            {
                RethrowInner(exception);
                throw;
            }
        }


        internal static bool TryAdd(
            long left,
            long right,
            string context,
            out long result)
        {
            result = 0L;
            if (!IsEnabled)
                return false;

            Resolve();
            if (add == null)
            {
                WarnUnavailable();
                return false;
            }

            try
            {
                result = (long)add.Invoke(
                    null,
                    new object[] { left, right, context });
                return true;
            }
            catch (TargetInvocationException exception)
            {
                RethrowInner(exception);
                throw;
            }
        }

        internal static bool TryMultiply(
            long left,
            long right,
            string context,
            out long result)
        {
            result = 0L;
            if (!IsEnabled)
                return false;

            Resolve();
            if (multiply == null)
            {
                WarnUnavailable();
                return false;
            }

            try
            {
                result = (long)multiply.Invoke(
                    null,
                    new object[] { left, right, context });
                return true;
            }
            catch (TargetInvocationException exception)
            {
                RethrowInner(exception);
                throw;
            }
        }

        internal static bool TryGetAverageEarnings(
            data_girls.girls girl,
            out long result)
        {
            result = 0L;
            if (!IsEnabled || girl == null)
                return false;

            Resolve();
            if (getAverageEarnings == null)
            {
                WarnUnavailable();
                return false;
            }

            try
            {
                result = (long)getAverageEarnings.Invoke(
                    null,
                    new object[] { girl });
                return true;
            }
            catch (TargetInvocationException exception)
            {
                RethrowInner(exception);
                throw;
            }
        }

        private static void Resolve()
        {
            if (resolved)
                return;

            resolved = true;

            Type continuation = AccessTools.TypeByName(ContinuationTypeName);
            if (continuation != null)
            {
                roundSingleProductCompatible = AccessTools.Method(
                    continuation,
                    "RoundSingleProductCompatible",
                    new Type[] { typeof(long), typeof(string), typeof(float[]) });
                getAverageEarnings = AccessTools.Method(
                    continuation,
                    "GetAverageEarnings",
                    new Type[] { typeof(data_girls.girls) });
            }

            Type repair = AccessTools.TypeByName(RepairTypeName);
            if (repair != null)
            {
                add = AccessTools.Method(
                    repair,
                    "Add",
                    new Type[] { typeof(long), typeof(long), typeof(string) });
                multiply = AccessTools.Method(
                    repair,
                    "Multiply",
                    new Type[] { typeof(long), typeof(long), typeof(string) });
            }
        }

        private static void WarnUnavailable()
        {
            if (warned)
                return;

            warned = true;
            Debug.LogWarning(
                "[Tel Mod Library] Save n Load Fixes is patched in, but its current wide-numeric helper surface could not be resolved. " +
                "Falling back to the mod's vanilla-compatible arithmetic for this calculation.");
        }

        private static void RethrowInner(TargetInvocationException exception)
        {
            Exception inner = exception.InnerException;
            if (inner == null)
                throw exception;

            ExceptionDispatchInfo.Capture(inner).Throw();
        }
    }
}
