/**
 * Wrapper for PointerEvent for use with confirm dialog.
 */
class ConfirmClickEvent extends PointerEvent {
    /**
     * True if event has already been confirmed.
     */
    isConfirmed = false;
}

export default ConfirmClickEvent;
