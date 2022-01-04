import { 
  CheckCircleIcon, 
  ExclamationCircleIcon,
  ExclamationIcon,
  InformationCircleIcon
} from "@heroicons/react/solid";

var classNames = require("classnames");

/**
 * component to show alert messages.
*/
export default function Alert({ message, type }) {

  const divClasses = classNames(
    "w-full p-3 rounded-md flex flex-row",
    { "bg-red-200": type === "error" },
    { "text-red-500": type === "error" },
    { "bg-green-200": type === "success" },
    { "text-green-500": type === "success" },
    { "bg-yellow-200": type === "warning" },
    { "text-yellow-500": type === "warning" },
    { "bg-blue-200": type === "info" },
    { "text-blue-500": type === "info" }
  );

  return (
    <div className={divClasses}>
      {type === "error" && <ExclamationCircleIcon className="h-7 w-7 mr-3" />}
      {type === "success" && <CheckCircleIcon className="h-7 w-7 mr-3" />}
      {type === "warning" && <ExclamationIcon className="h-7 w-7 mr-3" />}
      {type === "info" && <InformationCircleIcon className="h-7 w-7 mr-3" />}
      <span>{message.toString()}</span>
    </div>
  );
}
