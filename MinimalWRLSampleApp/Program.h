#include <wrl.h>
#include <Windows.UI.Xaml.h>

class Program :
	public Microsoft::WRL::RuntimeClass<ABI::Windows::UI::Xaml::IApplicationOverrides>
{
	InspectableClass(L"MinimalWRLSampleApp.Program", TrustLevel::BaseTrust);

private:
	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IApplicationOverrides> _baseImplementation;

public:
	Program(void);
	~Program(void);

	void SetBaseImplementation(ABI::Windows::UI::Xaml::IApplicationOverrides* pBaseImplementation);
	HRESULT STDMETHODCALLTYPE OnActivated(ABI::Windows::ApplicationModel::Activation::IActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnLaunched(ABI::Windows::ApplicationModel::Activation::ILaunchActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnFileActivated(ABI::Windows::ApplicationModel::Activation::IFileActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnSearchActivated(ABI::Windows::ApplicationModel::Activation::ISearchActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnShareTargetActivated(ABI::Windows::ApplicationModel::Activation::IShareTargetActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnFileOpenPickerActivated(ABI::Windows::ApplicationModel::Activation::IFileOpenPickerActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnFileSavePickerActivated(ABI::Windows::ApplicationModel::Activation::IFileSavePickerActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnCachedFileUpdaterActivated(ABI::Windows::ApplicationModel::Activation::ICachedFileUpdaterActivatedEventArgs *args);
	HRESULT STDMETHODCALLTYPE OnWindowCreated(ABI::Windows::UI::Xaml::IWindowCreatedEventArgs *args);
};
