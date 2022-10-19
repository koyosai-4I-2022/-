using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // ���݂̏��
    PlayState state;

    // �Q�[�����̃p�l��
    [SerializeField]
    GameObject playingPanel;
    // �v���C���[������(�Q�[�����n�܂�O�̑ҋ@���)�p�̃p�l��
    [SerializeField]
    GameObject inputSelectingPanel;
    // ���[�h���̕\���p�p�l��
    [SerializeField]
    GameObject RoadingPanel;
    // ���U���g�\���p�̃p�l��
    [SerializeField]
    GameObject resultingPanel;
    // �����L���O�\���p�̃p�l��
    [SerializeField]
    GameObject rankingPanel;

    // �Q�[����ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] PlayingText;
    [SerializeField]
    Image[] PlayingImage;

    // �ҋ@�I����ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] InputSelectingText;
    [SerializeField]
    Image[] InputSelectingImage;

    // ���[�h��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] RoadingText;
    [SerializeField]
    Image[] RoadingImage;

    // ���U���g��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] ResultingText;
    [SerializeField]
    Image[] ResultingImage;

    // �����L���O��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] RankingName;
    [SerializeField]
    Text[] RankingScore;
    [SerializeField]
    Image[] RankingImage;

    bool[] stateInit;

    void Start()
    {
        state = PlayState.Ranking;
        InitUI();
    }

    async void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
		{
            var result = await ServerRequestController.GetRanking();
            string s = "";
            foreach(var user in result.Users)
            {
                s += user.name + "\n";
            }
            Debug.Log(s);
        }
        switch(state)
        {
            // �Q�[����ʂł̖��t���[������
            case PlayState.Playing:
                // 
                if(!stateInit[0])
                    InitPlayerUI();
                UpdatePlayingUI();
                break;
            // ���f���̏���
            case PlayState.Paused:
                
                break;
            // ���͑I����ʂ̏���
            case PlayState.InputSelecting:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[1])
                    InitInputSelectingUI();
                UpdateInputSelectingUI();
                break;
            // ���[�h��ʂ̏���
            case PlayState.Roading:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[2])
                    InitRoadingUI();
                UpdateRoadingUI();
                break;
            // ���U���g��ʂ̏���
            case PlayState.Resulting:
                //  �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[3])
                    InitResultingUI();
                UpdateResultingUI();
                break;
            // �����L���O��ʂ̏���
            case PlayState.Ranking:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[4])
                    InitRankingUI();
                UpdateRankingUI();
                
                break;
            default:
                break;
        }
    }
    // UI�̏����ݒ�
    void InitUI()
	{
        stateInit = new bool[5];
	}

    // �Q�[�����̕`��X�V
    void UpdatePlayingUI()
	{

	}
    // �Q�[����ʂ̏����ݒ�
    void InitPlayerUI()
	{
        stateInit[0] = true;
	}

    // �ҋ@��ʂ̕`��X�V
    void UpdateInputSelectingUI()
	{

	}
    // �ҋ@��ʂ̏����ݒ�
    void InitInputSelectingUI()
	{
        stateInit[1] = true;
	}

    // ���[�h��ʂ̕`��X�V
    void UpdateRoadingUI()
	{

	}
    // ���[�h��ʂ̏����ݒ�
    void InitRoadingUI()
	{
        stateInit[2] = true;
	}

    // ���U���g�̕`��X�V
    void UpdateResultingUI()
    {

    }
    // ���U���g��ʂ̏����ݒ�
    void InitResultingUI()
	{
        stateInit[3] = true;
	}

    // �����L���O��ʂ̕`��X�V
    void UpdateRankingUI()
	{
        
	}
    // �����L���O��ʂ̏����ݒ�
    async void InitRankingUI()
	{
        stateInit[4] = true;

        var result = await ServerRequestController.GetRanking();

        for(int i = 0;i < 8; i++)
		{

		}
	}


    // ���݂̏�Ԃ�\���񋓌^
    // 0 �Q�[����
    // 1 ���f��
    // 2 ���[�h��
    // 3 �ҋ@���
    // 4 ���U���g
    // 5 �����L���O
    public enum PlayState
	{
        Playing = 0,
        Paused = 1,
        Roading = 2,
        InputSelecting = 3,
        Resulting = 4,
        Ranking = 5
	}
}
