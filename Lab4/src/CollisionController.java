import javafx.application.Platform;
import javafx.concurrent.Task;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.Button;
import javafx.scene.control.TextArea;
import java.util.Random;

public class CollisionController {
    private static final int MAX_ATTEMPTS = 10;
    private static final char COLLISION_SYMBOL = '+';
    private static final int COLLISION_DURATION = 10;
    private static final int SLOT_TIME = 35;

    private byte buffer = 0;

    @FXML private Button sendButton;
    @FXML private TextArea inputZone;
    @FXML private TextArea outputZone;
    @FXML private TextArea debugZone;

    private void send(byte data) {
        this.buffer = data;
    }

    private boolean isCollision() {
        return (System.currentTimeMillis() % 2) == 1;
    }

    boolean isPackageMode() {
        return false;
    }

    private boolean isChannelFree() {
        return (System.currentTimeMillis() % 2) == 1;
    }

    private void sleep(int millis) {
        try {
            Thread.sleep(millis);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    private static void runOnUIThread(Runnable task) {
        if(task == null) throw new NullPointerException("Task can't be null.");
        if(Platform.isFxApplicationThread()) task.run();
        else Platform.runLater(task);
    }

    private class SendTask extends Task<Void> {

        int i = 1;

        @Override
        protected Void call() {
            runOnUIThread(() -> {
                inputZone.setEditable(false);
                sendButton.setDisable(true);
            });

            byte[] line = inputZone.getText().getBytes();

            for (byte symbol: line) {
                StringBuilder collisions = new StringBuilder();
                int attempts = 0;
                boolean sending = true;
                if (!isPackageMode()) {
                    while (sending) {
                        if (isChannelFree()) {
                            send(symbol);
                            sleep(COLLISION_DURATION);

                            if (isCollision()) {
                                collisions.append(COLLISION_SYMBOL);
                                attempts += 1;

                                if (attempts >= MAX_ATTEMPTS) {
                                sending = false;
                                } else {
                                    Random rand = new Random();
                                    int k = Math.min(attempts, MAX_ATTEMPTS);
                                    int r = rand.nextInt((int) Math.pow(2, k));
                                    sleep(r * SLOT_TIME);
                                    }
                            } else {
                                runOnUIThread(() -> {
                                    outputZone.appendText((char) symbol + "");
                                    debugZone.appendText("" + i++ + ". ");
                                    //i++;
                                    debugZone.appendText(collisions + "\n");
                                });
                                sending = false;
                            }
                        }
                    }
                }
            }
            runOnUIThread(() -> {
                inputZone.setEditable(true);
                sendButton.setDisable(false);
            });
            inputZone.setText("");
            outputZone.appendText("\n");
            return null;
        }
    }

    @FXML protected void onSend(ActionEvent event) { new Thread(new SendTask()).start(); }
}