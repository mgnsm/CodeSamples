#include "Program.h"
#include <assert.h>

Program::Program() {}

Program::~Program() {}

void Program::SetBaseImplementation(ABI::Windows::UI::Xaml::IApplicationOverrides* pBaseImplementation)
{
	_baseImplementation = pBaseImplementation;
}

HRESULT Program::OnActivated(ABI::Windows::ApplicationModel::Activation::IActivatedEventArgs * args)
{
	return _baseImplementation->OnActivated(args);
}

HRESULT Program::OnLaunched(ABI::Windows::ApplicationModel::Activation::ILaunchActivatedEventArgs *args)
{
	/* TextBlock textBlock = new TextBlock(); */
	Microsoft::WRL::ComPtr<IInspectable> inspectableClass;
	HRESULT hr = RoActivateInstance(
		Microsoft::WRL::Wrappers::HStringReference(RuntimeClass_Windows_UI_Xaml_Controls_TextBlock).Get(),
		inspectableClass.ReleaseAndGetAddressOf());
	assert(SUCCEEDED(hr));

	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::Controls::ITextBlock> textBlock;
	hr = inspectableClass.As<ABI::Windows::UI::Xaml::Controls::ITextBlock>(&textBlock);
	assert(SUCCEEDED(hr));

	/* textBlock.Text = "Hello world!"; */
	hr = textBlock->put_Text(Microsoft::WRL::Wrappers::HStringReference(L"Hello world!").Get());
	assert(SUCCEEDED(hr));

	/* textBlock.FontSize = 40; */
	hr = textBlock->put_FontSize(40);
	assert(SUCCEEDED(hr));

	/* Cast the TextBlock to a FrameworkElement to be able to set the HorizontalAlignment and VerticalAlignment properties */
	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IFrameworkElement> frameworkElement;
	hr = inspectableClass.As<ABI::Windows::UI::Xaml::IFrameworkElement>(&frameworkElement);
	assert(SUCCEEDED(hr));

	/* textBlock.VerticalAlignment = VerticalAlignment.Center; */
	hr = frameworkElement->put_VerticalAlignment(ABI::Windows::UI::Xaml::VerticalAlignment::VerticalAlignment_Center);
	assert(SUCCEEDED(hr));

	/* textBlock.HorizontalAlignment = HorizontalAlignment.Center; */
	hr = frameworkElement->put_HorizontalAlignment(ABI::Windows::UI::Xaml::HorizontalAlignment::HorizontalAlignment_Center);
	assert(SUCCEEDED(hr));

	/* Window.Current */
	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IWindowStatics> window;
	hr = Windows::Foundation::GetActivationFactory(
		Microsoft::WRL::Wrappers::HStringReference(RuntimeClass_Windows_UI_Xaml_Window).Get(),
		&window);
	assert(SUCCEEDED(hr));

	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IWindow> currentWindow;
	hr = window->get_Current(&currentWindow);
	assert(SUCCEEDED(hr));

	/* Cast the TextBlock to a UIElement to be able to set the Window.Current property to it */
	Microsoft::WRL::ComPtr<ABI::Windows::UI::Xaml::IUIElement> uiElement;
	hr = inspectableClass.As<ABI::Windows::UI::Xaml::IUIElement>(&uiElement);
	assert(SUCCEEDED(hr));

	/* Window.Current.Content = textBlock; */
	hr = currentWindow->put_Content(uiElement.Get());
	assert(SUCCEEDED(hr));

	/* Window.Current.Activate(); */
	hr = currentWindow->Activate();
	assert(SUCCEEDED(hr));

	return hr;
}

HRESULT Program::OnFileActivated(ABI::Windows::ApplicationModel::Activation::IFileActivatedEventArgs * args)
{
	return _baseImplementation->OnFileActivated(args);
}

HRESULT Program::OnSearchActivated(ABI::Windows::ApplicationModel::Activation::ISearchActivatedEventArgs * args)
{
	return _baseImplementation->OnSearchActivated(args);
}

HRESULT Program::OnShareTargetActivated(ABI::Windows::ApplicationModel::Activation::IShareTargetActivatedEventArgs * args)
{
	return _baseImplementation->OnShareTargetActivated(args);
}

HRESULT Program::OnFileOpenPickerActivated(ABI::Windows::ApplicationModel::Activation::IFileOpenPickerActivatedEventArgs * args)
{
	return _baseImplementation->OnFileOpenPickerActivated(args);
}

HRESULT Program::OnFileSavePickerActivated(ABI::Windows::ApplicationModel::Activation::IFileSavePickerActivatedEventArgs * args)
{
	return _baseImplementation->OnFileSavePickerActivated(args);
}

HRESULT Program::OnCachedFileUpdaterActivated(ABI::Windows::ApplicationModel::Activation::ICachedFileUpdaterActivatedEventArgs * args)
{
	return _baseImplementation->OnCachedFileUpdaterActivated(args);
}

HRESULT Program::OnWindowCreated(ABI::Windows::UI::Xaml::IWindowCreatedEventArgs * args)
{
	return _baseImplementation->OnWindowCreated(args);
}