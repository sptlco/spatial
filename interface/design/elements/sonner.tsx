// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon, Spinner } from "..";
import { ExternalToast, toast as sonnerToast, Toaster as Primitive, ToasterProps } from "sonner";

/**
 * An element that displays toast notifications to the user.
 */
export const Toaster = createElement<typeof Primitive, ToasterProps>(({ closeButton = false, ...props }, ref) => (
  <Primitive
    {...props}
    ref={ref}
    position="top-center"
    closeButton={closeButton}
    offset={{
      top: 24,
      left: 40,
      right: 40,
      bottom: 24
    }}
    mobileOffset={{
      top: 24,
      left: 40,
      right: 40,
      bottom: 24
    }}
    icons={{
      info: <Icon symbol="info" className="font-light" />,
      error: <Icon symbol="emergency_home" className="font-light" />,
      loading: <Spinner className="size-4" />,
      success: <Icon symbol="check" className="font-light" />,
      warning: <Icon symbol="warning" className="font-light" />
    }}
    toastOptions={{
      classNames: {
        toast: "bg-blue! backdrop-blur! border-none! shadow-base! rounded-xl! gap-4! px-5! pointer-events-auto!",
        icon: "size-6! inline-flex items-center justify-center ml-0.5! text-white!",
        content: "mr-auto!",
        title: "text-white! text-xs! font-bold!",
        description: "text-white! text-xs!",
        actionButton: "transition-all! duration-300! rounded-lg! text-xs! text-white! bg-button! hover:bg-button-hover! active:bg-button-active!",
        cancelButton:
          "transition-all! duration-300! rounded-lg! text-xs! text-white! bg-button-ghost! hover:bg-button-ghost-hover! active:bg-button-ghost-active!",
        closeButton: "relative! shrink-0! order-999! transform-none! bg-transparent! text-white! border-none! scale-175! size-5!"
      }
    }}
  />
));

type PromiseT<Data = any> = Promise<Data> | (() => Promise<Data>);
interface PromiseIExtendedResult extends ExternalToast {
  message: React.ReactNode;
}
type PromiseTExtendedResult<Data = any> = PromiseIExtendedResult | ((data: Data) => PromiseIExtendedResult | Promise<PromiseIExtendedResult>);
type PromiseTResult<Data = any> = string | React.ReactNode | ((data: Data) => React.ReactNode | string | Promise<React.ReactNode | string>);
type PromiseExternalToast = Omit<ExternalToast, "description">;
type PromiseData<ToastData = any> = PromiseExternalToast & {
  loading?: string | React.ReactNode;
  success?: PromiseTResult<ToastData> | PromiseTExtendedResult<ToastData>;
  error?: PromiseTResult | PromiseTExtendedResult;
  description?: PromiseTResult;
  finally?: () => void | Promise<void>;
};

type titleT = (() => React.ReactNode) | React.ReactNode;

/**
 * Display a toast notification.
 * @param message The message to display.
 * @param data Configurable options for the message.
 * @returns The toast identifier.
 */
export const toast: ((message: titleT, data?: ExternalToast) => string | number) & {
  success: (message: titleT | React.ReactNode, data?: ExternalToast) => string | number;
  info: (message: titleT | React.ReactNode, data?: ExternalToast) => string | number;
  warning: (message: titleT | React.ReactNode, data?: ExternalToast) => string | number;
  error: (message: titleT | React.ReactNode, data?: ExternalToast) => string | number;
  custom: (jsx: (id: number | string) => React.ReactElement, data?: ExternalToast) => string | number;
  message: (message: titleT | React.ReactNode, data?: ExternalToast) => string | number;
  promise: <ToastData>(
    promise: PromiseT<ToastData>,
    data?: PromiseData<ToastData>
  ) =>
    | (string & {
        unwrap: () => Promise<ToastData>;
      })
    | (number & {
        unwrap: () => Promise<ToastData>;
      })
    | {
        unwrap: () => Promise<ToastData>;
      };
  dismiss: (id?: number | string) => string | number;
  loading: (message: titleT | React.ReactNode, data?: ExternalToast) => string | number;
} = sonnerToast as any;
