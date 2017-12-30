#include <Windows.h>
#include <assert.h>
#include <wrl.h>
#include "Program.h"

int __stdcall wWinMain(HINSTANCE, HINSTANCE, PWSTR, int)
{
	/* initialize WinRT */
	HRESULT hr = RoInitialize(RO_INIT_MULTITHREADED);
	assert(SUCCEEDED(hr));

	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IApplicationInitializationCallback> callback = 
		Microsoft::WRL::Callback<ABI::Windows::UI::Xaml::IApplicationInitializationCallback>([](ABI::Windows::UI::Xaml::IApplicationInitializationCallbackParams*)
	{
		Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IApplicationFactory> applicationFactory;
		HRESULT hr = Windows::Foundation::GetActivationFactory(
			Microsoft::WRL::Wrappers::HStringReference(RuntimeClass_Windows_UI_Xaml_Application).Get(),
			&applicationFactory);
		assert(SUCCEEDED(hr));
		
		Microsoft::WRL::ComPtr<Program> outer = Microsoft::WRL::Make<Program>();
		IInspectable* inner;
		Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IApplication> instance;
		hr = applicationFactory->CreateInstance(outer.Get(), &inner, instance.GetAddressOf());
		assert(SUCCEEDED(hr));

		Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IApplicationOverrides> baseApp;
		hr = inner->QueryInterface(__uuidof(baseApp), &baseApp);
		assert(SUCCEEDED(hr));
		outer->SetBaseImplementation(baseApp.Get());
		
		return S_OK;
	});

	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IApplicationStatics> applicationStatics;
	hr = Windows::Foundation::GetActivationFactory(
		Microsoft::WRL::Wrappers::HStringReference(RuntimeClass_Windows_UI_Xaml_Application).Get(),
		&applicationStatics);
	assert(SUCCEEDED(hr));

	/* Application.Start(callback); */
	hr = applicationStatics->Start(callback.Get());
	assert(SUCCEEDED(hr));

	return 0;
}