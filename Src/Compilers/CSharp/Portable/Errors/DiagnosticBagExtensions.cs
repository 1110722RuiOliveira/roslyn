﻿// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.CodeAnalysis.CSharp
{
    internal static class DiagnosticBagExtensions
    {
        /// <summary>
        /// Add a diagnostic to the bag.
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="code"></param>
        /// <param name="location"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static CSDiagnosticInfo Add(this DiagnosticBag diagnostics, ErrorCode code, Location location, params object[] args)
        {
            var info = new CSDiagnosticInfo(code, args, ImmutableArray<Symbol>.Empty, ImmutableArray<Location>.Empty);
            var diag = new CSDiagnostic(info, location);
            diagnostics.Add(diag);
            return info;
        }

        internal static CSDiagnosticInfo Add(this DiagnosticBag diagnostics, ErrorCode code, Location location, ImmutableArray<Symbol> symbols, params object[] args)
        {
            var info = new CSDiagnosticInfo(code, args, symbols, ImmutableArray<Location>.Empty);
            var diag = new CSDiagnostic(info, location);
            diagnostics.Add(diag);
            return info;
        }

        internal static void Add(this DiagnosticBag diagnostics, DiagnosticInfo info, Location location)
        {
            var diag = new CSDiagnostic(info, location);
            diagnostics.Add(diag);
        }

        /// <summary>
        /// Adds diagnostics from useSiteDiagnostics into diagnostics and returns True if there were any errors.
        /// </summary>
        internal static bool Add(
            this DiagnosticBag diagnostics,
            CSharpSyntaxNode node,
            HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            return !useSiteDiagnostics.IsNullOrEmpty() && diagnostics.Add(node.Location, useSiteDiagnostics);
        }

        internal static bool Add(
            this DiagnosticBag diagnostics,
            Location location,
            HashSet<DiagnosticInfo> useSiteDiagnostics)
        {
            if (useSiteDiagnostics.IsNullOrEmpty())
            {
                return false;
            }

            bool haveErrors = false;

            foreach (var info in useSiteDiagnostics)
            {
                if (info.Severity == DiagnosticSeverity.Error)
                {
                    haveErrors = true;
                }

                diagnostics.Add(new CSDiagnostic(info, location));
            }

            return haveErrors;
        }
    }
}