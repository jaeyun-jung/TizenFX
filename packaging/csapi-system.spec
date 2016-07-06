%{!?dotnet_assembly_path: %define dotnet_assembly_path %{_datadir}/assembly}

%if 0%{?tizen_build_devel_mode}
%define BUILDCONF Debug
%else
%define BUILDCONF Release
%endif

Name:       csapi-system
Summary:    Tizen System API for C#
Version:    1.0.0
Release:    1
Group:      Development/Libraries
License:    Apache-2.0
URL:        https://www.tizen.org
Source0:    %{name}-%{version}.tar.gz
Source1:    %{name}.manifest

# Mono
BuildRequires: mono-compiler
BuildRequires: mono-devel

# P/Invoke Build Requires
BuildRequires: pkgconfig(capi-system-device)
BuildRequires: pkgconfig(capi-system-runtime-info)
BuildRequires: pkgconfig(capi-system-info)
BuildRequires: pkgconfig(storage)

# C# API Requires
BuildRequires: csapi-tizen
BuildRequires: csapi-uifw

%description
Tizen System API for C#

%prep
%setup -q
cp %{SOURCE1} .

%define Assemblies Tizen.System

%build
for ASM in %{Assemblies}; do
xbuild $ASM/$ASM.csproj \
        /p:Configuration=%{BUILDCONF} \
        /p:ReferencePath=%{dotnet_assembly_path}
done

%install
# Assemblies
mkdir -p %{buildroot}%{dotnet_assembly_path}
for ASM in %{Assemblies}; do
install -p -m 644 $ASM/bin/%{BUILDCONF}/$ASM.dll %{buildroot}%{dotnet_assembly_path}
done

# License
mkdir -p %{buildroot}%{_datadir}/license
cp LICENSE %{buildroot}%{_datadir}/license/%{name}

%files
%manifest %{name}.manifest
%attr(644,root,root) %{dotnet_assembly_path}/*.dll
%attr(644,root,root) %{_datadir}/license/%{name}
